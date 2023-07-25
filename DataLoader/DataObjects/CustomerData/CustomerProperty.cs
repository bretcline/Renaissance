using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaxis.POS.CustomerData
{
    public partial class CustomerProperty 
    {
        public string ArchivePath => $"{UploadPath}\\Archive\\";
    }
}
