using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaxis.POS.Data
{
    public partial class vwPOSTicket
    {
        public decimal TicketAverage
        {
            get
            {
                var average = 0.0M;
                if( TicketTotal.HasValue && GuestCountModified.HasValue && GuestCountModified > 0 )
                {
                    average = TicketTotal.Value / GuestCountModified.Value * 1.0M;
                }
                return average;
            }
        }
    }
}
