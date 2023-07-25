using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelLoaders
{
    public class MicrosImport : CSVBulkLoad
    {

        public void LoadData(string fileName, string tableName)
        {


            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["RenAix"].ConnectionString;
            var conn = new SqlConnection(connString);

            var dt = new DataTable();

            var sqlTool = new Jaxis.Utilities.Database.SqlTool(conn);

            var param = new Jaxis.Utilities.Database.SqlParameterList();
            param.AddInParameter("@DataTable", tableName);
            param.AddInParameter("@DataSource", fileName);

            sqlTool.ExecuteProc("[procCleanupData]", param);
            
            sqlTool.ExecuteQuery(ref dt, tableName, string.Format("SELECT TOP( 0 ) * FROM imp.{0}", tableName));

            LoadData(conn, fileName, dt, '|', 10000, "imp", 2 );

        }
    }
}
