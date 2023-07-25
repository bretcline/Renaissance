using ReportGenerator.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            var builder = new DailyPOSItemSummary(connectionString);

            var report = builder.BuildReport();

            var emailer = new ReportPublisher();
            //emailer.SendReportViaEmail( report, builder.DailySummary );
        }
    }
}
