using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Jaxis.Data.Service;

namespace DataLoaderService
{
    public class WorkerObject
    {
        private ServiceHost m_DataLoader = null;
        private bool m_Running = false;

        public void Start()
        {
            if (null == m_DataLoader)
                m_DataLoader = new ServiceHost(typeof(DataLoaderWCF));
            else
                m_DataLoader.Close( );

            m_DataLoader.Open();
            m_Running = true;
            while (m_Running)
            {
                Task.Delay(250);
            }
        }

        public void Stop()
        {
            m_DataLoader?.Close(new TimeSpan(2500));
            m_Running = false;
        }
    }
}