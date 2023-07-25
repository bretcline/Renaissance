using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaxis.Utilities.Database;

namespace ReportGenerator.Reports
{
    public abstract class BaseSqlReport
    {
        protected SqlTool m_Conn;

        protected BaseSqlReport(string _connectionString)
        {
            m_Conn = new SqlTool(_connectionString);
        }

        public abstract string BuildReport();
    }
}
