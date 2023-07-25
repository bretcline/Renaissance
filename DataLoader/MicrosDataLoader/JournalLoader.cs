using Jaxis.Util.Log4Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Micros.DataLoader.Parsers;
using System.Globalization;
using System.Threading;
using JaxisExtensions;
using SubSonic.DataProviders;

namespace Micros.DataLoader
{
    public class JournalLoader
    {
        protected static string LOG_TYPE = "JournalLoader";
        protected IParser m_Parser = null;
        Action m_UpdateBar = null;
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }


        public JournalLoader( string configFile, bool appendToTicket, string connectionString, string providerName = "" )
        {
            ConnectionString = SubSonic.DataProviders.ProviderFactory.ConnectionString = connectionString;
            ProviderName = SubSonic.DataProviders.ProviderFactory.ProviderName = providerName;

            m_Parser = new Generic(configFile, appendToTicket);
        }

        public void LoadJournal(string path, Action updateBar = null)
        {
            m_UpdateBar = updateBar;
            ReadFile(path);
        }

        private void ReadFile(string filePath)
        {
            Log.Wrap<int>(LOG_TYPE, "JournalLoader::ReadFile", LogType.Debug, false, () =>
            {
                {
                    var KeysToRemove = new Queue<string>();
                    {
                        var files = Directory.GetFiles(filePath, "*.JNL");
                        foreach (var file in files)
                        {
                            LoadFile(file);
                        }
                    }
                }
                return 1;
            });
        }

        public void LoadFile(string file)
        {
            var dataSource = Path.GetFileName(file);

            try
            {
                var storedProcs = new Jaxis.POS.Data.RenAixDB( ConnectionString, ProviderName );
                var proc = storedProcs.procCleanupData("POSTickets", dataSource);
                proc.Execute();
            }
            catch (Exception err)
            {
                Log.Exception(err);
            }

            var success = true;
            m_Parser.ClearCache();
//            using (var reader = new StreamReader(file, Encoding.GetEncoding("iso-8859-1")))
            using (var reader = new StreamReader(file, Encoding.Default))
            {
                Log.Info(file);

                UpdateProcgressBar();

                var builder = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    builder.Append(ProcessFile(reader));
                    {
                        if (null != m_Parser && builder.Length > 0)
                        {
                            try
                            {
                                Log.Debug(LOG_TYPE, builder.ToString());
                                var T = m_Parser.ParseData(builder.ToString());
                                T.DataSource = dataSource;
                                Log.Debug(LOG_TYPE, $"POSDriver::ProduceMessage() {System.Environment.NewLine}{T.ToString()}");

                                if (T.CheckNumber == null)
                                {
                                    Log.Debug("Null Check Number");
                                }
                                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadedSaver), T);

                                //SaveMessage(T);
                            }
                            catch (Exception err)
                            {
                                Log.Exception(LOG_TYPE, err);
                                Log.Info(LOG_TYPE, builder.ToString());
                                success = false;
                            }
                            builder.Clear();
                        }
                        else if (null == m_Parser)
                        {
                            Log.Error("Parser is Invalid!");
                        }
                    }
                }
            }
            if (success)
            {
                File.Move(file, file + ".processed");
            }
        }

        private void ThreadedSaver( object ticket )
        {
            SaveMessage(ticket as ITicket);
        }

        public void LoadJournalFile(string path)
        {
            throw new NotImplementedException();
        }

        private void SaveMessage(ITicket _Ticket)
        {
            try
            {
                var ticketDate = null == _Ticket.Date ? DateTime.Now : Convert.ToDateTime(_Ticket.Date, new CultureInfo("fr-ca"));

                Jaxis.POS.Data.POSTicket ticket = null;
                if (_Ticket.TouchCount > 1)
                {
                    var items = Jaxis.POS.Data.POSTicket.All().Where(x => x.CheckNumber == _Ticket.CheckNumber).ToList();
                    var item = items.FirstOrDefault(x => x.TicketDate.Date == ticketDate.Date);

                    //var item = (from n in items
                    //            group n by n.POSTicketID into g
                    //            select g.OrderByDescending(t => t.TicketDate).FirstOrDefault()).FirstOrDefault();

                    ticket = item;
                }
                if (null == ticket)
                {
                    ticket = new Jaxis.POS.Data.POSTicket();
                }

                // This is a duplicate ticket...if the transaction number is the same what we already have, it's a duplicate.
                if (ticket.CheckNumber == _Ticket.CheckNumber && ticket.TransactionID == _Ticket.TransactionID)
                {
                    ticket.TouchCount = _Ticket.TouchCount;
                    ticket.RawData = _Ticket.GetRawData();
                    ticket.TicketTotal = _Ticket.CheckTotal;
                    ticket.TipAmount = _Ticket.TipAmount;
                    ticket.TransactionType = _Ticket.TransactionType;
                    ticket.Save();
                }
                else
                {
                    ticket.TouchCount = _Ticket.TouchCount;
                    ticket.CheckNumber = _Ticket.CheckNumber;
                    ticket.Comments = _Ticket.Comments;
                    ticket.CustomerTable = _Ticket.Table;
                    ticket.Establishment = _Ticket.Establishment;
                    ticket.GuestCount = _Ticket.GuestCount;
                    ticket.Server = _Ticket.ServerNumber;
                    ticket.ServerName = _Ticket.Server;
                    ticket.TicketDate = ticketDate;
                    ticket.RawData = _Ticket.GetRawData();
                    ticket.TicketTotal = _Ticket.CheckTotal;
                    ticket.DataSource = _Ticket.DataSource;
                    ticket.TransactionID = _Ticket.TransactionID;
                    ticket.TipAmount = _Ticket.TipAmount;
                    ticket.TransactionType = _Ticket.TransactionType;

                    ticket.Save();

                    foreach (var tvaItem in _Ticket.TVAItems)
                    {
                        if (ticket.TouchCount > 1)
                        {
                            var tva =
                                Jaxis.POS.Data.POSTVADatum.All()
                                    .FirstOrDefault(
                                        t => t.POSTicketID == ticket.POSTicketID && t.Percentage == tvaItem.Percentage);
                            if (null != tva)
                            {
                                tva.Amount = tvaItem.Amount;
                                tva.Total = tvaItem.Total;
                                tva.Save();
                            }
                        }
                        else
                        {
                            var tva = new Jaxis.POS.Data.POSTVADatum
                            {
                                POSTicketID = ticket.POSTicketID,
                                Amount = tvaItem.Amount,
                                Percentage = (decimal?)tvaItem.Percentage,
                                Total = tvaItem.Total
                            };

                            tva.Save();
                        }
                    }

                    foreach (var paymentItem in _Ticket.PaymentItems)
                    {
                        if (ticket.TouchCount > 1)
                        {
                            var payment =
                                Jaxis.POS.Data.POSPaymentDatum.All()
                                    .FirstOrDefault(
                                        t =>
                                            t.POSTicketID == ticket.POSTicketID &&
                                            t.AccountNumber ==
                                            paymentItem.AccountNumber);
                            if (null != payment)
                            {
                                payment.CustomerName = paymentItem.CustomerName;
                                payment.Payment = paymentItem.Payment;
                                payment.PaymentType = paymentItem.PaymentType;
                                payment.RoomNumber = paymentItem.RoomNumber;

                                payment.Save();
                            }
                        }
                        else
                        {
                            var payment = new Jaxis.POS.Data.POSPaymentDatum
                            {
                                POSTicketID = ticket.POSTicketID,
                                AccountNumber =
                                                  paymentItem.AccountNumber,
                                CustomerName = paymentItem.CustomerName,
                                Payment = paymentItem.Payment,
                                PaymentType = paymentItem.PaymentType,
                                RoomNumber = paymentItem.RoomNumber
                            };

                            payment.Save();
                        }
                    }
                    //if (_Ticket.Items.Count > 5 * _Ticket.GuestCount)
                    //{
                    //    Console.WriteLine($"Way to many ticket items {_Ticket.Items.Count} for {_Ticket.GuestCount} on ticket {_Ticket.CheckNumber}");
                    //}


                    foreach (var ticketItem in _Ticket.Items)
                    {
                        var ti = new Jaxis.POS.Data.POSTicketItem();
                        ti.POSTicketID = ticket.POSTicketID;
                        ti.Comment = ticketItem.Comment;
                        ti.Description = ticketItem.Description;
                        ti.Price = Convert.ToDecimal(ticketItem.Price, CultureInfo.InvariantCulture);
                        ti.Quantity = ticketItem.Quantity;
                        ti.Credit = ticketItem.Credit;

                        ti.Save();

                        foreach (var m in ticketItem.Modifiers)
                        {
                            var modifier = new Jaxis.POS.Data.POSTicketItemModifier
                            {
                                Name = m.Name,
                                Price = Convert.ToDecimal(m.Price, CultureInfo.InvariantCulture),
                                POSTicketItemID = ti.POSTicketItemID
                            };
                            modifier.Save();
                        }
                    }
                }
                UpdateProcgressBar();
            }
            catch (Exception exp)
            {
                Log.WriteException("TicketConsumer::ConsumeTicket", exp);
            }
        }

        private void UpdateProcgressBar()
        {
            if (null != m_UpdateBar)
            {
                m_UpdateBar();
            }
        }

        private string ProcessFile(StreamReader _reader)
        {
            var builder = new StringBuilder();
            try
            {
                //bool ticketComplete = false;
                bool isTicket = false;

                while (!_reader.EndOfStream)
                {
                    var line = _reader.ReadLine();//.ReplaceDiacritics( );
                    if (line != null)
                    {
                        if (line.StartsWith("Tbl"))
                        {
                            isTicket = true;
                        }
                        else if (line.Contains("================================") && isTicket)
                        {
                            break;
                        }
                        if (isTicket)
                        {
                            builder.Append($"{line}{Environment.NewLine}");
                        }
                    }

                }
            }
            catch (Exception err)
            {
                Log.Exception(LOG_TYPE, err);
            }
            return builder.ToString();
        }

    }
}
