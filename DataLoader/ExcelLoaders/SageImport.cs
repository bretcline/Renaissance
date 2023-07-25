using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelLoaders
{
    public class SageImport
    {
        private static readonly ILog m_Logger = LogManager.GetLogger(typeof(SageImport));

        public void LoadSageData( string fileName, DateTime accountingPeriod)
        {
            var file = new FileInfo(fileName);

            using (var package = new ExcelPackage(file))
            {
                try
                {
                    var workbook = package.Workbook;
                    //for( int s = workbook.Worksheets.Count; s > 0; --s )
                    {
                        var sheet = workbook.Worksheets["SageBalance"];


                        m_Logger.Debug(sheet.Name);
                        try
                        {
                            var storedProcs = new Jaxis.POS.Data.RenAixDB();
                            var proc = storedProcs.procCleanupData("SageBalance", file.FullName);
                            proc.Execute();

                            var rowCount = sheet.Dimension.End.Row + 1;
                            {
                                var columns = sheet.Dimension.End.Column;
                                if (0 < columns)
                                {
                                    for (var i = 2; i < rowCount; ++i)
                                    {

                                        try
                                        {
                                            //AccountCode	Category	Description	DescriptionEng	Debit	Credit	Sales

                                            var line = new Jaxis.POS.Data.SageBalance();
                                            line.DataSource = file.FullName;
                                            line.AccountingPeriod = accountingPeriod;

                                            line.AccountCode = Int64.Parse( sheet.Cells[i, 1].GetValue<string>() );
                                            line.Category = sheet.Cells[i, 2].GetValue<int>();
                                            line.CategoryName = sheet.Cells[i, 3].GetValue<string>();
                                            line.CategoryNameEng = sheet.Cells[i, 4].GetValue<string>();
                                            line.Debit = sheet.Cells[i, 5].GetValue<decimal>();
                                            line.Credit = sheet.Cells[i, 6].GetValue<decimal>();
                                            line.SalesRecorded = sheet.Cells[i, 7].GetValue<decimal>();


                                            line.Save();




                                            //var wellName = sheet.Cells[i, 2].GetValue<string>();
                                            //var api = sheet.Cells[i, 3].GetValue<string>();
                                            //if (!string.IsNullOrWhiteSpace(api))
                                            //{
                                            //    var well =
                                            //        wells.Select(string.Format("API10 = '{0}'",
                                            //                                     api.Substring(0, 10)));
                                            //    if (0 < well.Length)
                                            //    {
                                            //        AddRow(data, well, columns, sheet, i);
                                            //    }
                                            //    else
                                            //    {
                                            //        AddRow(data, well, columns, sheet, i);
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    var well =
                                            //       wells.Select(string.Format("WellName = '{0}'",
                                            //                                    wellName));
                                            //    //if( 0 < well.Length )
                                            //    {
                                            //        AddRow(data, well, columns, sheet, i);
                                            //    }

                                            //}
                                        }
                                        catch (Exception err)
                                        {
                                            m_Logger.Error(err.Message);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            m_Logger.Error(err.Message);
                        }
                    }
                }
                catch (Exception err)
                {
                    m_Logger.Error(err.Message);
                }
            }

            //if (data.Rows.Count > 0)
            //{
            //    sqlCommand.CommandText = "TRUNCATE TABLE imp.WellActivitySchedule";
            //    sqlCommand.ExecuteNonQuery();
            //}
            //DataUtils.GeneralUtils.BulkInsert(sqlConn, data, "imp.WellActivitySchedule");
        }
    }
}
