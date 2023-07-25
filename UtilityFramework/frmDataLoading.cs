using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilityFramework
{
    public partial class frmDataLoading : Form
    {
        protected static ILog m_Logger = LogManager.GetLogger(typeof(frmDataLoading));

        protected BlockingCollection<string> m_MessageList = new BlockingCollection<string>(); 
        public frmDataLoading()
        {
            InitializeComponent();
        }

        public TextBox LoggingWindow { get; set; }
        private void frmDataLoading_Load(object sender, EventArgs e)
        {
        }


        protected void ShowWaitDialog(DoWorkEventHandler method)
        {
            this.pictureBox.Image = UtilityFramework.Properties.Resources.Animation11;
            this.pictureBox.Visible = true;

            var processor = new BackgroundWorker();

            processor.DoWork += method;// 

            processor.RunWorkerCompleted += (a, b) =>
            {
                if (b.Error != null)
                {
                    MessageBox.Show(b.Error.Message);
                }
                else
                {
                    MessageBox.Show("Data load complete");
                }
                this.pictureBox.Visible = false;
            };

            processor.RunWorkerAsync();
        }

        protected void tmrMessageTimer_Tick(object sender, EventArgs e)
        {
            var builder = new StringBuilder();
            string message;
            while (m_MessageList.TryTake(out message))
            {
                builder.Append(string.Format("{0}{1}", message, Environment.NewLine));
            }
            LoggingWindow.AppendText(builder.ToString());
        }
    }
}
