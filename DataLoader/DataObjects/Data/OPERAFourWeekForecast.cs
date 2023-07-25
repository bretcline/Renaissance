using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaxis.POS.Data
{
    public partial class OPERAFourWeekForecast
    {
        public void SetData( string delimitedData, char delimiter )
        {
            var items = delimitedData.Split(delimiter);

        }

    }
}
