using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using JaxisExtensions;
using System.IO;
using System.Xml.Schema;
using Jaxis.Util.Log4Net;

namespace Micros.DataLoader.Parsers
{
    public class Generic : IParser
    {
        protected bool m_AppendDateTimeToTicket = true;
        protected ParserConfig m_Config = null;

        protected Dictionary<string, ITicket> m_Tickets = new Dictionary<string, ITicket>();

        public Generic( string _ConfigFile, bool appendToTicket )
        {
            m_AppendDateTimeToTicket = appendToTicket;
            if (File.Exists(_ConfigFile))
            {
                using (StreamReader Reader = new StreamReader(_ConfigFile))
                {
                    var FileData = Reader.ReadToEnd();
                    m_Config = FileData.DeserializeObject<ParserConfig>();

                    m_Config.Cleanup = (from LP in m_Config.Cleanup orderby LP.ParseOrder select LP).ToList();

                    for (int i = 0; i < m_Config.SectionParsers.Count; ++i)
                    {
                        m_Config.SectionParsers[i] = (from LP in m_Config.SectionParsers[i] orderby LP.ParseOrder select LP).ToList();
                    }
                }
            }
            else
            {
                Log.Error( string.Format("Parser Config {0} does not exist.", _ConfigFile));
            }
        }


        #region IParser Members

        public void ClearCache()
        {
            m_Tickets.Clear();
        }

        public ITicket ParseData( string _Data )
        {
            return Log.Wrap<ITicket>( "ParseData", LogType.Debug, false, ( ) =>
            {
                var rc = new Ticket();
                rc.Items = new List<ITicketItem>();

                SetTransactionTypes(rc, _Data);

                rc.RawData = new List<string>(Regex.Split(_Data, System.Environment.NewLine));
                Log.Debug( string.Format( "{0}{1}", System.Environment.NewLine, _Data ) );
                if (null != m_Config)
                {
                    var sections = Regex.Split(_Data, m_Config.SectionMarker).ToList();
                    Log.Debug(string.Format("Section Count {0}", sections.Count()));

                    var sectionCount = sections.Count;
                    for (int i = sectionCount - 1; i >= 0; --i)
                    {
                        if (string.IsNullOrWhiteSpace(sections[i]))
                        {
                            sections.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < sections.Count; ++i)
                    {
                        if (!string.IsNullOrWhiteSpace(sections[i]))
                        {
                            var section = sections[i];
                            //Log.Debug(string.Format("Section {2}{0}{1}", System.Environment.NewLine, Section, i));
                            try
                            {
                                foreach (LineParser LP in m_Config.Cleanup)
                                {
                                    section = Regex.Replace(section, LP.RegEx, string.Empty);
                                }
                                Log.Debug(string.Format("Section {2}{0}{1}", System.Environment.NewLine, section, i));
                                ParseSection(rc, i, section, m_Config.SectionParsers[i], m_AppendDateTimeToTicket);
                            }
                            catch (Exception err)
                            {
                                Log.WriteException("Parsing Sections", err);
                            }
                        }
                    }
                }
                if (m_Tickets.ContainsKey(rc.CheckNumber))
                {
                    if (rc.CheckNumber == "5936")
                    {
                        var r = rc.CheckNumber.ToString();
                        Log.Debug(r);
                    }

                    m_Tickets[rc.CheckNumber].TouchCount++;
                    m_Tickets[rc.CheckNumber].Items.AddRange(rc.Items);
                    m_Tickets[rc.CheckNumber].TVAItems.AddRange(rc.TVAItems);
                    m_Tickets[rc.CheckNumber].PaymentItems.AddRange(rc.PaymentItems);
                    m_Tickets[rc.CheckNumber].Date = rc.Date;
                    m_Tickets[rc.CheckNumber].RawData.AddRange( rc.RawData );
                    if( rc.TipAmountModified )
                        m_Tickets[rc.CheckNumber].TipAmount = rc.TipAmount;
                    if( rc.CheckTotalModified )
                        m_Tickets[rc.CheckNumber].CheckTotal = rc.CheckTotal;
                    m_Tickets[rc.CheckNumber].TransactionType = rc.TransactionType;
                }
                else
                {
                    rc.TouchCount = 1;
                    m_Tickets.Add(rc.CheckNumber, rc);
                }

                return m_Tickets[rc.CheckNumber];
            } );
        }

        private void SetTransactionTypes(Ticket rc, string _Data)
        {
            try
            {
                foreach( var element in m_Config.TransactionTypes )
                {
                    if( Regex.IsMatch(_Data, element.TextIdentifier) )
                    {
                        rc.TransactionType = element.TransactionType;
                    }
                }
            }
            catch
            {

            }
        }

        private static int ParseSection( ITicket _rc, int i, string _section, IEnumerable<LineParser> _lines, bool _appendDateTimeToTicket )
        {
            var builder = new StringBuilder();
            var builder2 = new StringBuilder();
            foreach (char C in _section.ToCharArray())
            {
                builder.Append(string.Format("{0} ", (int)C));
                if (!char.IsControl(C))
                {
                    builder2.Append(C);
                }
                else
                {
                    builder2.Append(System.Environment.NewLine);
                }
            }
            _section = builder2.ToString();

            foreach( var parser in _lines )
            {
                var replacements = parser.GetReplacementValues();
                var parts = parser.GetElements();
                int ItemCount = 0;

                {
                    try
                    {

                        Log.Debug(string.Format("{0} - {1} - {2}", parser.PropertyName, parser.RegEx, _section));
                        var matches = Regex.Matches(_section, parser.RegEx);
                        foreach (Match m in matches)
                        {
                            for (i = 0; i < m.Groups.Count; ++i)
                            {
                                Group G = m.Groups[i];
                                Log.Debug(string.Format("{0} - {1}", i, G.Value));
                            }

                            if (parser.PropertyName.Equals("LineItem"))
                            {
                                ItemCount = ProcessLineItem(_rc, m, parser, parts, ItemCount);
                            }
                            else if (parser.PropertyName.Equals("TAVData"))
                            {
                                ProcessTVAData(_rc, m, parser, parts);
                            }
                            else if (parser.PropertyName.Equals("TipData"))
                            {
                                ProcessTipData(_rc, m, parser, parts);
                            }
                            else if ( parser.PropertyName.Equals("PaymentData") )
                            {
                                ProcessPaymentData(_rc, m, parser, parts);
                            }
                            else
                            {
                                ProcessTicketData(_rc, _appendDateTimeToTicket, parts, m, replacements);
//                                break;
                            }
                            if (parser.SingleElement == true)
                            {
                                break;
                            }

                            _section = _section.Replace($"{m.Value}", "");
                        }
                    }
                    catch (Exception err)
                    {
                        Log.Exception(err);
                    }
                }
            }
            return i;
        }

        private static void ProcessTipData(ITicket _rc, Match m, LineParser parser, Dictionary<string, int> parts)
        {
            Log.Debug(string.Format("Tip Data Parse{0}{1}{0}{2}", System.Environment.NewLine, m.Value, parser.RegEx));
            try
            {
                foreach (var key in parts.Keys)
                {
                    switch (key)
                    {
                        case "TipAmount":
                            {
                                _rc.TipAmount = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                                    ? 0.0M
                                                    : decimal.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);
                                break;
                            }
                    }
                }
            }
            catch (Exception err)
            {
                Log.Exception(err);
            }
        }

        private static void ProcessPaymentData(ITicket _rc, Match m, LineParser parser, Dictionary<string, int> parts)
        {
            Log.Debug(string.Format("Payment Data Parse{0}{1}{0}{2}", System.Environment.NewLine, m.Value, parser.RegEx));
            try
            {
                //AcountNumber=1,RoomNumber=2,CustomerName=3,PaymentType=4,Payment=5
                var item = new PaymentData();

                foreach (var key in parts.Keys)
                {
                    switch (key)
                    {
                        case "AcountNumber":
                        {
                            item.AccountNumber = m.Groups[parts[key]].Value;
                            break;
                        }
                        case "RoomNumber":
                        {
                            item.RoomNumber = m.Groups[parts[key]].Value;
                            break;
                        }
                        case "CustomerName":
                        {
                            item.CustomerName = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                ? string.Empty
                                : m.Groups[parts[key]].Value;
                            break;
                        }
                        case "PaymentType":
                        {
                            item.PaymentType = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                ? string.Empty
                                : m.Groups[parts[key]].Value;
                            break;
                        }
                        case "Payment":
                        {
                            decimal payment = 0.0M;
                            if (!string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value))
                            {
                                decimal.TryParse(m.Groups[parts[key]].Value, out payment);
                            }
                            item.Payment = payment;
                            break;
                        }
                    }
                }
                _rc.PaymentItems.Add(item);
            }
            catch (Exception err)
            {
                Log.Exception(err);
            }
        }

        private static void ProcessTVAData(ITicket _rc, Match m, LineParser parser, Dictionary<string, int> parts)
        {
            Log.Debug(string.Format("TVA Data Parse{0}{1}{0}{2}", System.Environment.NewLine, m.Value, parser.RegEx));
            try
            {
                var item = new TVAData();

                foreach (var key in parts.Keys)
                {
                    switch (key)
                    {
                        case "TVAAmount":
                            {
                                item.Amount = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                                    ? 0.0M
                                                    : decimal.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);
                                break;
                            }
                        case "TVAPercent":
                            {
                                item.Percentage = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                                    ? 0.0M
                                                    : decimal.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);
                                break;
                            }
                        case "TVATotal":
                            {
                                item.Total = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                                    ? 0.0M
                                                    : decimal.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);
                                break;
                            }
                    }
                }
                _rc.TVAItems.Add(item);
            }
            catch (Exception err)
            {
                Log.Exception(err);
            }
        }

        private static void ProcessTicketData(ITicket _rc, bool _appendDateTimeToTicket, Dictionary<string, int> parts, Match m,
                                              Dictionary<string, string> replacements)
        {
            foreach (var key in parts.Keys)
            {

                try
                {
                    switch (key)
                    {
                        case "Establishment":
                        {
                            var value = m.Groups[parts[key]].Value;
                            if (null == replacements || 0 == replacements.Keys.Count || !replacements.ContainsKey(value))
                            {
                                _rc.Establishment = m.Groups[parts[key]].Value;
                            }
                            else
                            {
                                _rc.Establishment = replacements[value];
                            }
                            break;
                        }
                        case "Guest":
                        {
                            _rc.GuestCount = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                ? 1
                                : int.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);
                            break;
                        }
                        case "Check":
                        {
                            var checkNumber = m.Groups[parts[key]].Value;
                            if (!string.IsNullOrWhiteSpace(checkNumber))
                            {
                                if (_appendDateTimeToTicket)
                                {
                                    var time = DateTime.Now.ToString("M/d/yy hh:mm:ss");
                                    if (!string.IsNullOrWhiteSpace(_rc.Date))
                                    {
                                        time = _rc.Date;
                                    }
                                    _rc.CheckNumber = string.Format("{0} - {1}", checkNumber, time);
                                }
                                else
                                {
                                    _rc.CheckNumber = checkNumber;
                                }
                            }
                            break;
                        }
                        case "Table":
                        {
                            _rc.Table = m.Groups[parts[key]].Value;
                            break;
                        }
                        case "TicketTime":
                        {
                            //30Sep'15 10:04
                            try
                            {
                                _rc.Date = m.Groups[parts[key]].Value.Trim();
                                _rc.Date = Regex.Replace(_rc.Date, "'", " ");
                                _rc.Date = Regex.Replace(_rc.Date, " {2,}", " ");
                                _rc.Date = Regex.Replace(_rc.Date, "Aou", "Aug");
                                _rc.Date = Regex.Replace(_rc.Date, "Avr", "Apr");
                                _rc.Date = Regex.Replace(_rc.Date, "Fev", "Feb");
                                _rc.Date = Regex.Replace(_rc.Date, "Mai", "May");
                                _rc.Date = Regex.Replace(_rc.Date, "Mai", "May");

                                _rc.Date =
                                    DateTime.ParseExact(_rc.Date, "ddMMM yy HH:mm", CultureInfo.InvariantCulture)
                                        .ToString("MM/dd/yyyy HH:mm");
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                            //string Date = DateTime.Now.ToString("yyyy-MM-dd {0}:ss");
                            //_rc.Date = string.Format(Date, m.Value);
                            break;
                        }
                        case "Transaction":
                        {
                            _rc.TransactionID = m.Groups[parts[key]].Value.Trim();
                            break;
                        }
                        case "Server":
                        {
                            _rc.Server = m.Groups[parts[key]].Value;
                            break;
                        }
                        case "ServerNumber":
                        {
                            _rc.ServerNumber = m.Groups[parts[key]].Value;
                            break;
                        }
                        case "Total":
                        {
                            _rc.CheckTotal = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                ? 1
                                : decimal.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);

                            break;
                        }
                    }
                }
                catch (Exception err)
                {
                    Log.Exception(err);
                }
            }
        }

        private static int ProcessLineItem(ITicket _rc, Match m, LineParser parser, Dictionary<string, int> parts,
            int ItemCount)
        {
            Log.Debug(string.Format("Line Item Parse{0}{1}{0}{2}", System.Environment.NewLine, m.Value, parser.RegEx));
            try
            {
                var valid = true;
                var item = new TicketItem();

                foreach (var key in parts.Keys)
                {
                    switch (key)
                    {
                        case "Quantity":
                        {
                            item.Quantity = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                ? 1
                                : int.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);
                            break;
                        }
                        case "Description":
                        {
                            string value = string.Empty;
                            if (!string.IsNullOrWhiteSpace(parser.DescriptionPrefix))
                            {
                                value += parser.DescriptionPrefix;
                            }
                            value += m.Groups[parts[key]].Value.Trim();
                            if (!string.IsNullOrWhiteSpace(parser.DescriptionSuffix))
                            {
                                value += parser.DescriptionSuffix;
                            }

                            item.Description = value.Trim();
                            if (string.IsNullOrWhiteSpace(item.Description))
                            {
                                valid = false;
                            }

                            break;
                        }
                        case "Modifier":
                        {
                            if (m.Groups.Count > parts[key] && !string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value))
                            {
                                item.Modifiers = new List<ITicketItemModifier>();
                                var mod = new TicketItemModifier();
                                mod.Name = m.Groups[parts[key]].Value.Trim();
                                item.Modifiers.Add(mod);
                            }

                            break;
                        }
                        case "Price":
                        {
                            item.Price = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                ? (0.0M == item.Price) ? 0.0M : item.Price
                                : decimal.Parse(m.Groups[parts[key]].Value, CultureInfo.InvariantCulture);
                            break;
                        }
                        case "Credit":
                        {
                            item.Credit = string.IsNullOrWhiteSpace(m.Groups[parts[key]].Value)
                                ? false
                                : true;
                            break;
                        }
                    }
                }
                if (valid)
                {
                    _rc.Items.Add(item);
                    ++ItemCount;
                }
            }
            catch (Exception err)
            {
                Log.Exception(err);
            }
            return ItemCount;
        }

        #endregion

        protected void GenerateConfigFile( string _OutputFile )
        {

            var Config = new ParserConfig( );

            Config.Cleanup = new List<LineParser>( );
            Config.Cleanup.Add( new LineParser( ) { RegEx = @"Redirect from:\n.*", SingleElement = true, ParseOrder = 1 } );
            Config.Cleanup.Add( new LineParser( ) { RegEx = @"\*\*\* END OF CHECK \*\*\*", SingleElement = true, ParseOrder = 2 } );
            Config.Cleanup.Add( new LineParser( ) { RegEx = @"^\n", ParseOrder = 0, SingleElement = false } );

            Config.SectionMarker = @"\*\*\*.*\*\*\*";
            Config.SectionParsers = new List<List<LineParser>>( );

            var Header = new List<LineParser>( );
            Header.Add( new LineParser( ) { PropertyName = "Establishment", RegEx = @".+\n", ParseOrder = 0, SingleElement = true } );
            Header.Add( new LineParser( ) { PropertyName = "Check", RegEx = @"(Check: )([0-9]+)", ParseOrder = 1, SingleElement = true } );
            Header.Add( new LineParser( ) { PropertyName = "Table", RegEx = @"(Table: )(.*)", ParseOrder = 2, SingleElement = true } );
            Header.Add( new LineParser( ) { PropertyName = "TicketTime", RegEx = @" \d\d:\d\d", ParseOrder = 3, SingleElement = true } );

            Config.SectionParsers.Add( Header );

            var LineItems = new List<LineParser>( );
            LineItems.Add( new LineParser( ) { RegEx = @"(\n \d \w*.*\n)(    \w*.*\n)+", ParseOrder = 0, SingleElement = false } );

            Config.SectionParsers.Add( LineItems );

            using( StreamWriter Writer = new StreamWriter( _OutputFile ) )
            {
                Writer.SerializeObject( Config );
            }
        }
    }
}
