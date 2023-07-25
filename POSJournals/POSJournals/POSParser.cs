using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSJournals
{
    class POSParser
    {

        public StreamReader DataStream { get; set; }

        public string TicketSeparator { get; set; }

        public string SectionSeparator { get; set; }

        protected List<String> m_Tickets = new List<string>();

        public POSParser( )
        {

        }

        public POSParser( StreamReader _reader )
        {
            DataStream = _reader;
        }

        public void ParseData( )
        {
            SeparateTickets();

            ParsetTickets();
        }

        private void ParsetTickets()
        {
            foreach( var ticket in m_Tickets )
            {
                ProcessTicket(ticket);
            }
        }

        private void ProcessTicket(string ticket)
        {
            var sections = ticket.Split((char)224);

            var lines = ticket.Split((char)223);
            foreach ( var line in lines )
            {
                Console.WriteLine(line);

            }
        }

        private void SeparateTickets()
        {
            var ticket = new StringBuilder();
            var line = DataStream.ReadLine();
            do
            {
                if (line.Equals(TicketSeparator))
                {
                    m_Tickets.Add(ticket.ToString());
                    ticket.Clear();
                }
                if (line.Equals(SectionSeparator))
                {
                    ticket.Append(string.Format("{0}", (char)224));
                }
                else
                {
                    ticket.Append(string.Format("{0}{1}", line, (char)223));
                }
                line = DataStream.ReadLine();

            } while (line != null);
        }
    }
}
