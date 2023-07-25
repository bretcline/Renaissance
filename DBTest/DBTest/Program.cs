using Jaxis.Utilities.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RenAix"].ConnectionString;
            Console.WriteLine(ConnectionString);
            using (var sqlTool = new SqlTool(ConnectionString))
            {
                var reader = sqlTool.ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'imp' ORDER BY TABLE_NAME");
                Console.WriteLine(string.Format("Read the tables - {0}", reader.FieldCount ) );
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                }
            }

        }
    }
}
