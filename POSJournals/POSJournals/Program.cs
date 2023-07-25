using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSJournals
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var stream = new StreamReader(@"F:\Journals\aout\12082015.jnl"))
            {
                var parser = new POSParser(stream);

                parser.TicketSeparator = "================================";
                parser.SectionSeparator = "--------------------------------";
                parser.ParseData();

            }
        }
    }
}
