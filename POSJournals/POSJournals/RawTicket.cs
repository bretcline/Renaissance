using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSJournals
{

    public enum SectionTypes
    {
        Header,
        Items,
        Footer,
        
    }
    public class RawTicket
    {
        public RawTicket( string _data )
        {
            Sections = new Dictionary<SectionTypes, string>();


        }
        public Dictionary<SectionTypes, string> Sections { get; protected set; }
    }
}
