using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micros.DataLoader.Parsers
{
    public interface IParser
    {
        ITicket ParseData( string _Data );
        void ClearCache();
    }
}
