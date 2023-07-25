using FileHelpers;
using Jaxis.POS.Data;
using SubSonic.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace ExcelLoaders
{
    public class OperaImport : CSVBulkLoad
    {
        public void CreateDataClass(string fileName, string className)
        {
            var cvsClass = new FileHelpers.Dynamic.CsvClassBuilder(className, '\t', fileName);
            cvsClass.SaveToSourceFile(string.Format("{0}.cs", className));
        }

        public void LoadFourWeekForecastData(string fileName )
        {

            LoadData(fileName, "OPERAFourWeekForecast");
            
        }
        public void LoadResFutureOccupancyData(string fileName)
        {
            //var file = new FileInfo(fileName);

            //var engine = new FileHelperEngine<ResFutureOccupancy>();

            //// To Read Use:
            //var results = engine.ReadFile(fileName);
            //foreach (var item in results)
            //{
            //    var operaItem = item.ToType<OPERAResFutureOccupancy>();
            //    operaItem.DataSource = file.FullName;
            //    operaItem.Save();
            //}
        }

        public void LoadData( string fileName, string tableName )
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["RenAix"].ConnectionString;
            var conn = new SqlConnection(connString);

            DataTable dt = new DataTable( );

            var sqlTool = new Jaxis.Utilities.Database.SqlTool( conn );

            var param = new Jaxis.Utilities.Database.SqlParameterList();
            param.AddInParameter("@DataTable", tableName);
            param.AddInParameter("@DataSource", fileName);

            sqlTool.ExecuteProc("[procCleanupData]", param );

            sqlTool.ExecuteQuery(ref dt, tableName, string.Format( "SELECT TOP( 0 ) * FROM imp.{0}", tableName ) );

            LoadData(conn, fileName, dt, '\t');

        }
    }
}
