using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace DataLoaderService
{
    public partial class DataLoaderService : ServiceBase
    {
        private Thread m_ServiceTrd = null;
        private WorkerObject m_Worker = null;

        public DataLoaderService()
        {
            InitializeComponent();
#if DEBUG
            this.OnStart(null);
#endif
        }

        protected override void OnStart(string[] args)
        {
            m_Worker = new WorkerObject();
            m_ServiceTrd = new Thread(m_Worker.Start);
            m_ServiceTrd.Start();
        }

        protected override void OnStop()
        {
            m_Worker.Stop();
            m_ServiceTrd.Join(2500);
        }
    }
}
