using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace ReportGenerator.Reports
{
    public class DailyPOSBeverateSummary : BaseSqlReport
    {
        public DailyPOSBeverateSummary(string _connectionString) : base(_connectionString)
        {
        }

        public override string BuildReport()
        {
            var rc = "";
            //try
            //{
            //    var today = DateTime.Now;
            //    var fileName = $"DailyPOSItemSummary.{today.Day - 1}.{today.Month}.{today.Year}.xlsx";

            //    File.Delete(fileName);

            //    var query = "SELECT * FROM rpt.vwDailyPOSItemSummary";
            //    var table = new DataTable();
            //    m_Conn.ExecuteQuery(ref table, "vwDailyPOSItemSummary", query);

            //    var newFile = new FileInfo(fileName);

            //    using (var package = new ExcelPackage(newFile))
            //    {
            //        // Get unique Establishments
            //        GetSummaryData(package);
            //        GetBeverageUsageData(package);

            //        //                    GetTopTenData(package);

            //        var items = package.Workbook.Worksheets.Add("Items");
            //        var price = package.Workbook.Worksheets.Add("Price");

            //        var establishments = from p in table.AsEnumerable()
            //            group p by new { Field = p.Field<string>("ESTABLISHMENT") } //or group by new {p.ID, p.Name, p.Whatever}
            //            into mygroup
            //            select mygroup.FirstOrDefault();

            //        foreach (var establishment in establishments)
            //        {
            //            var sheet = package.Workbook.Worksheets.Add(establishment.Field<string>("ESTABLISHMENT"));
            //            //sheet.View.ShowGridLines = false;
            //            var rowOffset = 1;


            //            var results = (from myRow in table.AsEnumerable()
            //                where myRow.Field<string>("ESTABLISHMENT") == establishment.Field<string>("ESTABLISHMENT")
            //                select myRow).ToList();


            //            for (var i = 0; i < table.Columns.Count; ++i)
            //            {
            //                var column = table.Columns[i];
            //                sheet.SetValue(rowOffset, i + 1, column.ColumnName);
            //            }
            //            rowOffset++;
            //            for (var j = 0; j < results.Count; ++j)
            //            {
            //                for (var i = 0; i < table.Columns.Count; ++i)
            //                {
            //                    sheet.SetValue(rowOffset + j, i + 1, results[j][i]);
            //                }
            //            }

            //            sheet.Cells[$"A1:A{results.Count + 1}"].Style.Numberformat.Format = "yyyy-mm-dd";
            //            sheet.Cells[$"D1:D{results.Count + 1}"].Style.Numberformat.Format = "$#,##0.00";

            //            CreateChart(items, sheet, results.Count + 1, "C", "E");
            //            CreateChart(price, sheet, results.Count + 1, "C", "D");
            //            m_ChartCount++;

            //            sheet.Cells["A1:K20"].AutoFitColumns();
            //        }
            //        package.Save();
            //        rc = newFile.FullName;
            //    }
            //}
            //catch (Exception err)
            //{
            //    Console.WriteLine(err.Message);
            //}

            return rc;
        }

        private void GetBeverageUsageData(ExcelPackage _package)
        {
//            var sheet = _package.Workbook.Worksheets.Add("Beverage Summary");

//            var query = @"SELECT ESTABLISHMENT, PRODUCT, Name, SUM( TotalItems ) AS Total
//FROM[rpt].[vwPOSItemSummary]
//WHERE CAST(BUSINESS_DATE AS DATE ) = CAST(DATEADD(day, -1, GETDATE()) AS DATE) AND(MAJ_GRP_SEQ = '4' OR MAJ_GRP_SEQ = '2') AND TotalItems <> 0
//GROUP BY ESTABLISHMENT, PRODUCT, Name
//ORDER BY ESTABLISHMENT
//";
//            var table = new DataTable();
//            m_Conn.ExecuteQuery(ref table, "BeverageSummary", query);

//            var values = new Dictionary<string, decimal>();

//            var rowOffset = 1;
//            var rowCount = 0;
//            for (var j = 0; j < table.Rows.Count; ++j)
//            {
//                var row = table.Rows[j];
//                var establishemt = row["Establishment"].ToString();
//                try
//                {
//                    if (establishemt == "Daily Total")
//                    {
//                        CreateEmailSummary(row);
//                    }

//                    var columnOffset = 0;
//                    if (j % 2 == 0 && rowCount > 1)
//                    {
//                        columnOffset = 5;
//                        rowOffset -= 10;
//                    }

//                    values[establishemt] = Decimal.Parse(row["TicketTotal"].ToString());

//                    var guestCount = Convert.ToInt32(row["TotalGuests"]);
//                    var ticketTotal = Convert.ToDecimal(row["TicketTotal"]);

//                    sheet.Cells[rowOffset, 1 + columnOffset].Style.Font.Bold = true;
//                    sheet.SetValue(rowOffset++, 1 + columnOffset, establishemt);
//                    sheet.SetValue(rowOffset, 1 + columnOffset, "Ticket Count:");
//                    sheet.SetValue(rowOffset++, 2 + columnOffset, row["TicketCount"]);
//                    sheet.SetValue(rowOffset, 1 + columnOffset, "Total Guests:");
//                    sheet.SetValue(rowOffset++, 2 + columnOffset, guestCount);

//                    sheet.Cells[rowOffset, 2 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                    sheet.Cells[rowOffset, 4 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                    sheet.SetValue(rowOffset, 1 + columnOffset, "Minimum Ticket:");
//                    sheet.SetValue(rowOffset, 2 + columnOffset, row["MinTicket"]);
//                    sheet.SetValue(rowOffset, 3 + columnOffset, "Maximum Ticket:");
//                    sheet.SetValue(rowOffset++, 4 + columnOffset, row["MaxTicket"]);

//                    sheet.Cells[rowOffset, 2 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                    sheet.SetValue(rowOffset, 1 + columnOffset, "Sum of Discounts:");
//                    sheet.SetValue(rowOffset++, 2 + columnOffset, row["SumDiscounts"]);

//                    if (establishemt == "MiniBar" || establishemt == "Daily Total")
//                    {
//                        sheet.Cells[rowOffset, 2 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                        sheet.Cells[rowOffset, 4 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                        sheet.SetValue(rowOffset, 1 + columnOffset, "Minibar Lost:");
//                        sheet.SetValue(rowOffset, 2 + columnOffset, row["MinibarLost"]);
//                        sheet.SetValue(rowOffset, 3 + columnOffset, "Amenities:");
//                        sheet.SetValue(rowOffset++, 4 + columnOffset, row["Aminities"]);
//                    }

//                    sheet.Cells[rowOffset, 2 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                    sheet.Cells[rowOffset, 4 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                    sheet.SetValue(rowOffset, 1 + columnOffset, "Sum of Tickets:");
//                    sheet.SetValue(rowOffset, 2 + columnOffset, row["TicketTotal"]);
//                    sheet.SetValue(rowOffset, 3 + columnOffset, "Sum of Tips:");
//                    sheet.SetValue(rowOffset++, 4 + columnOffset, row["Tips"]);

//                    sheet.Cells[rowOffset, 2 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                    sheet.Cells[rowOffset, 4 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                    sheet.SetValue(rowOffset, 1 + columnOffset, "Sum of Credits:");
//                    sheet.SetValue(rowOffset, 2 + columnOffset, row["TotalCredits"]);
//                    sheet.SetValue(rowOffset, 3 + columnOffset, "Sum of Payments:");
//                    sheet.SetValue(rowOffset++, 4 + columnOffset, row["PaymentTotal"]);

//                    if (guestCount > 0)
//                    {
//                        sheet.Cells[rowOffset, 4 + columnOffset].Style.Numberformat.Format = "$#,##0.00";
//                        sheet.SetValue(rowOffset, 3 + columnOffset, "Average per Ticket:");
//                        sheet.SetValue(rowOffset++, 4 + columnOffset, ticketTotal / Convert.ToDecimal(guestCount));
//                    }

//                    rowOffset += 2;
//                    rowCount++;
//                }
//                catch (Exception err)
//                {
//                }
//            }

//            CreatePieChart(sheet, values, 6, "Revenue by Outlet");

//            sheet.Cells["A1:Z500"].AutoFitColumns();
        }

    }
}