using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaxis.Utilities.Database;
using System.Globalization;
using Jaxis.Util.Log4Net;
using OfficeOpenXml.FormulaParsing.Utilities;
using JaxisExtensions;

namespace ExcelLoaders
{
    public class CSVBulkLoad
    {
        public CultureInfo CultureToUse { get; set; }
        public CSVBulkLoad( )
        {
            CultureToUse = CultureInfo.InvariantCulture;
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RenAix"]?.ConnectionString;
        }

        public CSVBulkLoad( string connectionString )
        {
            CultureToUse = CultureInfo.InvariantCulture;
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }

        public IEnumerable<string> GetTableList( )
        {
            var tableList = new List<string>();
            try
            {
                using (var sqlTool = new SqlTool(ConnectionString))
                {
                    var reader = sqlTool.ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'imp' ORDER BY TABLE_NAME");
                    while (reader.Read())
                    {
                        tableList.Add(reader.GetString(0));
                    }
                }
            }
            catch( Exception err )
            {
                Log.Debug(err.Message);
            }
            return tableList;
        }

        public void ValidateTable( string schema, string tableName, List<string> columnNames, List<List<string>> dataValues, char  delimiter )
        {
            using (var sqlTool = new SqlTool(ConnectionString))
            {
                var query = string.Format("SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}'", schema, tableName);
                int tableCount = 0;
                sqlTool.ExecuteScalar(query, ref tableCount);
                if( 0 == tableCount )
                {
                    var sql = CreateTable(columnNames, dataValues, tableName, schema);

                    sqlTool.Execute(sql);
                }
            }
        }

        public void LoadData(string tableName, string fileName, char delimiter, int maxRows = 10000, string schema = "imp", int additionalColumns = 0, int headerColumns = 1)
        {
            try
            {
                Log.Debug("LoadData: " + fileName);
                using (var reader = new StreamReader(fileName, Encoding.Default))
                {
                    var dataSourceName = Path.GetFileName(fileName);
                    var fileData = reader.ReadToEnd().Replace( "\r", "" );//.ReplaceDiacritics();
                    var fileLines = new List<List<string>>( );

                    var lines = fileData.Split('\n');
                    var columns = lines[0].Split(delimiter).ToList<string>();
                    for( var i = 0; i < lines.Length; ++i )
//                    foreach ( var line in lines )
                    {
                        var line = lines[i];
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var items = line.Split(delimiter).ToList<string>();
                            if (items.Count < columns.Count &&
                                    i + 2 < lines.Length)
                            {
                                //items.AddRange(new string[columns.Count - items.Count]);
                                var subItems = lines[i + 1].Split(delimiter).ToList<string>();
                                if (i == 187)
                                {
                                    Console.WriteLine( "" );
                                }
                                while (columns.Count >= items.Count + subItems.Count )
                                {
                                    Log.Info($"Odd Line - {line}");

                                    line += "," + lines[i + 1];
                                    if (columns.Count >= items.Count + subItems.Count)
                                    {
                                        items.AddRange(subItems);
                                        i++;
                                    }
                                    if (i + 2 < lines.Length)
                                    {
                                        subItems = lines[i + 1].Split(delimiter).ToList<string>();
                                    }
                                }
                            }
                            items = line.Split(delimiter).ToList<string>();
                            if (items.Count < columns.Count)
                            {
                                items.AddRange(new string[columns.Count - items.Count]);
                            }
                            fileLines.Add(items);
                        }
                    }
                    for (int i = 0; i < headerColumns; ++i)
                    {
                        fileLines.RemoveAt(0);
                    }
//                    columns.RemoveRange(fileLines[0].Count, columns.Count - fileLines[0].Count);
                    if (headerColumns > 0)
                    {
                        ValidateTable(schema, tableName, columns, fileLines, delimiter);
                    }

                    var conn = new SqlConnection(ConnectionString);
                    var dt = new DataTable();
                    var sqlTool = new SqlTool(conn);

                    var param = new Jaxis.Utilities.Database.SqlParameterList();
                    param.AddInParameter("@DataTable", string.Format( "{0}.{1}", schema, tableName));
                    param.AddInParameter("@DataSource", dataSourceName);

                    Log.Debug("procCleanupData");
                    try
                    {
                        sqlTool.ExecuteProc("[procCleanupData]", param);
                    }
                    catch( Exception err )
                    {
                        Log.Exception(err);
                    }

                    Log.Debug($"Get Dataset for {"tableName"}");

                    var tableSchema = sqlTool.GetTableSchema($"imp.{tableName}");


                    sqlTool.ExecuteQuery(ref dt, tableName, $"SELECT TOP( 0 ) * FROM imp.{tableName}");
                    var suspectRows = dt.Clone();
                    int count = 0;
                    foreach (var values in fileLines)
                    {
                        ++count;

                        if (dt.Columns.Count == values.Count + additionalColumns)
                        {
                            DataRow row = dt.NewRow();
                            try
                            {
                                bool dataError = false;
                                for (int i = 0; i < dt.Columns.Count; ++i)
                                {
                                    var position = -1;

                                    string column = dt.Columns[i].ColumnName;
                                    if (headerColumns > 0)
                                    {
                                        position = columns.IndexOf(column);
                                    }
                                    else if (i < values.Count)
                                    {
                                        position = i;
                                    }
                                    try
                                    {

                                        if (dt.Columns[i].DataType == typeof(Guid))
                                        {
                                            row[column] = Guid.NewGuid();
                                        }

                                        if (column == "InsertDate")
                                        {
                                            row[column] = DateTime.Now;
                                        }
                                        else if (column == "DataSource")
                                        {
                                            row[column] = dataSourceName;
                                        }
                                        else if (0 <= position)
                                        {
                                            var data = values[position];

                                            if (!string.IsNullOrWhiteSpace(data))
                                            {
                                                if (data.Length > 250)
                                                {
                                                    Log.Debug(data);
                                                }
                                                try
                                                {
                                                    if (dt.Columns[i].DataType == typeof(int))
                                                    {
                                                        row[column] = Convert.ToInt32(data, CultureInfo.InvariantCulture);
                                                    }
                                                    else if (dt.Columns[i].DataType == typeof(decimal))
                                                    {
                                                        row[column] = Convert.ToDecimal(data, CultureInfo.InvariantCulture);
                                                    }
                                                    else if (dt.Columns[i].DataType == typeof(DateTime))
                                                    {
                                                        try
                                                        {
                                                            var date = Convert.ToDateTime(data, CultureToUse);
                                                            if (date < SqlDateTime.MinValue)
                                                            {
                                                                row[column] = SqlDateTime.MinValue;
                                                            }
                                                            else if (date > SqlDateTime.MaxValue)
                                                            {
                                                                row[column] = SqlDateTime.MaxValue;
                                                            }
                                                            else
                                                            {
                                                                row[column] = date;
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {
                                                            if (data.Length != "2016-03-08 10:55:03.000".Length)
                                                            {
                                                                Log.Info($"French Date Hack for certain values (? at the front of the value) - {data}");
                                                                data = data.Substring(1);
                                                                Log.Debug($"{column} - {data} - {CultureToUse.EnglishName}");
                                                            }
                                                            var date = Convert.ToDateTime(data, CultureToUse);
                                                            if (date < SqlDateTime.MinValue)
                                                            {
                                                                row[column] = SqlDateTime.MinValue;
                                                            }
                                                            else if (date > SqlDateTime.MaxValue)
                                                            {
                                                                row[column] = SqlDateTime.MaxValue;
                                                            }
                                                            else
                                                            {
                                                                row[column] = date;
                                                            }
                                                        }
                                                    }
                                                    else if (dt.Columns[i].DataType == typeof(double))
                                                    {
                                                        row[column] = Convert.ToDouble(data, CultureInfo.InvariantCulture);
                                                    }
                                                    else if (dt.Columns[i].DataType == typeof(Guid))
                                                    {
                                                        row[column] = new Guid(data);
                                                    }
                                                    else if (dt.Columns[i].DataType == typeof(TimeSpan))
                                                    {
                                                        row[column] = TimeSpan.Parse(data);
                                                    }
                                                    else if (dt.Columns[i].DataType == typeof(string))
                                                    {
                                                        int length = Convert.ToInt32(tableSchema.Rows[i][2]);
                                                        if (data.Length > length)
                                                        {
                                                            Log.Info( $"Column {column} is being truncated.");
                                                            data = data.Substring(0, length);
                                                        }
                                                        row[column] = data;
                                                    }
                                                    else
                                                    {
                                                        row[column] = data;
                                                    }
                                                }
                                                catch (Exception err)
                                                {
                                                    dataError = true;
                                                    Log.Debug($"{column} - {values[position]} - {err.Message} - {CultureToUse.EnglishName} - Line Number {count}");
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception err)
                                    {
                                        dataError = true;
                                        Log.Debug($"{column} - {values[position]} - {err.Message} - Line Number {count}");
                                    }
                                }
                                if (dt.Columns.Contains("CheckNumber"))
                                {
                                    if (row["CheckNumber"] == DBNull.Value)
                                        row["CheckNumber"] = 0;
                                }
                                if (dataError)
                                {
                                    suspectRows.Rows.Add(row.ItemArray);
                                    row.Delete();
                                }
                                else
                                {
                                    dt.Rows.Add(row);
                                }

                                if (0 != maxRows && dt.Rows.Count >= maxRows)
                                {
                                    BulkInsert(conn, dt, $"{schema}.{dt.TableName}");
                                }
                            }
                            catch (Exception err)
                            {
                                Log.Debug( $" Line Number {count} - {err.Message}");
                            }
                        }
                    }
                    BulkInsert(conn, dt, $"{schema}.{dt.TableName}");

                    if (0 < suspectRows.Rows.Count)
                    {
                        BulkInsert(conn, suspectRows, $"{schema}.{dt.TableName}");
                    }
                    //sqlTool.ExecuteProc("[procUpdateEstablishments]", null);                    
                }
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }
        }

        public void BulkInsert(SqlConnection sqlConn, DataTable dt, string tableName)
        {
            try
            {
                Log.Debug($"INSERTING: {dt.Rows.Count} rows in {tableName}");

                if (0 < dt.Rows.Count)
                {
                    var errors = dt.AsEnumerable().Where(r => r.HasErrors == true).ToList();
                    if (0 < errors.Count)
                    {
                        foreach (var dataRow in errors)
                        {
                            Log.Debug( dataRow.RowError );
                        }

                    }
                    using (var bulkCopy = new SqlBulkCopy(sqlConn))
                    {
                        bulkCopy.BulkCopyTimeout = 600;
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                Log.Debug(message);
            }
            dt.Clear();
        }

        public string CreateTable(DataTable table, string schema = "mrg", string tableName = "")
        {
            var sqlsc = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = table.TableName;
            }

            sqlsc.Append($"CREATE TABLE {schema}.{tableName}(");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sqlsc.Append(Environment.NewLine);
                sqlsc.Append(table.Columns[i].ColumnName);
                if (table.Columns[i].DataType.ToString().Contains("System.Int32"))
                {
                    sqlsc.Append(" int ");
                }
                else if (table.Columns[i].DataType.ToString().Contains("System.Int16"))
                {
                    sqlsc.Append(" int ");
                }
                else if (table.Columns[i].DataType.ToString().Contains("System.Int64"))
                {
                    sqlsc.Append(" BIGINT ");
                }
                else if (table.Columns[i].DataType.ToString().Contains("System.DateTime"))
                {
                    sqlsc.Append(" datetime ");
                }
                else if (table.Columns[i].DataType.ToString().Contains("System.String"))
                {
                    sqlsc.Append(table.Columns[i].MaxLength == 32
                        ? " UNIQUEIDENTIFIER "
                        : $" nvarchar({table.Columns[i].MaxLength}) ");
                }
                else if (table.Columns[i].DataType.ToString().Contains("System.Single"))
                {
                    sqlsc.Append(" decimal(10,5) ");
                }
                else if (table.Columns[i].DataType.ToString().Contains("System.Double"))
                {
                    sqlsc.Append(" decimal(10,5) ");
                }
                else if (table.Columns[i].DataType.ToString().Contains("System.Decimal"))
                {
                    sqlsc.Append(" decimal(18,10) ");
                }
                else
                {
                    sqlsc.Append($" nvarchar({table.Columns[i].MaxLength}) ");
                }

                if (table.Columns[i].AutoIncrement)
                {
                    sqlsc.Append($" IDENTITY({table.Columns[i].AutoIncrementSeed},{table.Columns[i].AutoIncrementStep}) ");
                }
                if (!table.Columns[i].AllowDBNull)
                {
                    sqlsc.Append(" NOT NULL ");
                }
                sqlsc.Append(",");
            }
            if (table.PrimaryKey.Length > 0)
            {
                string pks = $"{Environment.NewLine}CONSTRAINT PK_{tableName} PRIMARY KEY (";
                pks = table.PrimaryKey.Aggregate(pks, (current, t) => current + (t.ColumnName + ","));
                pks = pks.Substring(0, pks.Length - 1) + ")";

                sqlsc.Append(pks);
            }

            return sqlsc + ")";
        }

        public string CreateTable(List<string> columnNames, List<List<string>> dataValues, string tableName, string schema = "imp")
        {
            var sqlsc = new StringBuilder();
            try
            {

                sqlsc.Append(String.Format("CREATE TABLE {0}.{1}(", schema, tableName));
                for (int i = 0; i < columnNames.Count; i++)
                {
                    Type type = typeof(string);

                    var length = 0;
                    if (string.IsNullOrWhiteSpace(dataValues[0][i]))
                    {
                        var count = 0;
                        for (int j = 1; j < dataValues.Count; ++j)
                        {
                            if (!string.IsNullOrWhiteSpace(dataValues[j][i]))
                            {
                                type = DetectDataType(dataValues[j][i]);
                                if (type != typeof(string))
                                {
                                    break;
                                }
                                ++count;
                                length += dataValues[j][i].Length;
                            }
                        }
                        if (length == 0)
                        {
                            type = typeof(string);
                            length = 255;
                        }
                        else
                        {
                            length /= count;
                        }
                    }
                    else
                    {
                        type = DetectDataType(dataValues[0][i]);
                        length += dataValues[0][i].Length;
                    }


                    var dataType = type.ToString();
                    sqlsc.Append(Environment.NewLine);
                    sqlsc.Append(columnNames[i]);
                    if (dataType.Contains("System.Int32"))
                    {
                        sqlsc.Append(" int ");
                    }
                    else if (dataType.Contains("System.Int16"))
                    {
                        sqlsc.Append(" int ");
                    }
                    else if (dataType.Contains("System.Int64"))
                    {
                        sqlsc.Append(" BIGINT ");
                    }
                    else if (dataType.Contains("System.DateTime"))
                    {
                        sqlsc.Append(" datetime ");
                    }
                    else if (dataType.Contains("System.String"))
                    {
                        sqlsc.Append(length == 32
                            ? String.Format(" UNIQUEIDENTIFIER ")
                            : String.Format(" nvarchar({0}) ", ( length * 4 ) < 255 ? 255 : length * 4 ));
                    }
                    else if (dataType.Contains("System.Single"))
                    {
                        sqlsc.Append(" decimal(10,5) ");
                    }
                    else if (dataType.Contains("System.Double"))
                    {
                        sqlsc.Append(" decimal(10,5) ");
                    }
                    else if (dataType.Contains("System.Decimal"))
                    {
                        sqlsc.Append(" decimal(18,10) ");
                    }
                    else
                    {
                        sqlsc.Append(String.Format(" nvarchar({0}) ", (length * 4) < 255 ? 255 : length * 4 ));
                    }
                    sqlsc.Append(",");
                }

                var additionalColumns = string.Format("[{0}ID] [uniqueidentifier] NOT NULL,{1}[DataSource] [nvarchar](255) NOT NULL,{1}CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED {1}({1}[{0}ID] ASC{1})WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]{1}) ON [PRIMARY] ", tableName, System.Environment.NewLine);
                sqlsc.Append(additionalColumns);

            }
            catch (Exception er)
            {
                throw er;
            }
            return sqlsc.ToString( );
        }

        private Type DetectDataType (string str)
        {

            bool boolValue;
            Int32 intValue;
            Int64 bigintValue;
            decimal decimalValue;
            double doubleValue;
            DateTime dateValue;
            Guid guidValue;

            // Place checks higher in if-else statement to give higher priority to type.

            if (bool.TryParse(str, out boolValue))
                return boolValue.GetType();
            else if ( str.Contains( "." ) && decimal.TryParse(str, out decimalValue))
                return decimalValue.GetType( );
            else if ( str.Contains(".") && double.TryParse(str, out doubleValue))
                return doubleValue.GetType();
            else if (Int32.TryParse(str, out intValue))
                return intValue.GetType();
            else if (Int64.TryParse(str, out bigintValue))
                return bigintValue.GetType();
            else if (DateTime.TryParse(str, out dateValue))
                return dateValue.GetType();
            else if (Guid.TryParse(str, out guidValue))
                return guidValue.GetType();
            else return typeof( string );

        }


    }
}
