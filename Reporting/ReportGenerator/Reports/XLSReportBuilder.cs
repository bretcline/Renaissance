using Jaxis.Utilities.Database;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Reports
{
    public class XLSReportBuilder
    {
        SqlTool m_Conn;

        public XLSReportBuilder( string _connectionString )
        {
            m_Conn = new SqlTool(_connectionString);
        }

        public void DailyPOSData( )
        {
            try
            {
                var query = "SELECT * FROM rpt.vwDailyPOSItemSummary )";
                var table = new DataTable();
                m_Conn.ExecuteQuery(ref table, "vwDailyPOSItemSummary", query );

                var newFile = new FileInfo("DailyPOSItemSummary.xlsx");


                using (var package = new ExcelPackage(newFile))
                {
                    var ws = package.Workbook.Worksheets.Add("Data");
                    ws.View.ShowGridLines = false;
                    int rowOffset = 1;
                 
                    
                    //Headers
                    for (int i = 0; i < table.Columns.Count; ++i)
                    {
                        var column = table.Columns[i];
                        ws.SetValue(rowOffset, i + 1, column.ColumnName);
                        //ws.Column( i + 1 ).Style.Numberformat = 
                    }
                    rowOffset++;
                    for (int j = 0; j < table.Rows.Count; ++j)
                    {
                        for (int i = 0; i < table.Columns.Count; ++i)
                        {
                            var column = table.Columns[i];
                            ws.SetValue(rowOffset + j, i + 1, table.Rows[j][i].ToString( ) );
                        }
                    }

                    package.Save();
                }
            }
            catch ( Exception err )
            {

            }
        }

    }
}
