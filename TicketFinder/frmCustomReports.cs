using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketFinder
{
    public partial class frmCustomReports : Form
    {
        svcTicketFinder.TicketFinderClient m_Service = null;
        private Guid m_UserSession;

        public frmCustomReports( Guid _userSession )
        {
            InitializeComponent();

            m_UserSession = _userSession;
        }

        private void frmCustomReports_Load(object sender, EventArgs e)
        {
            m_Service = new TicketFinder.svcTicketFinder.TicketFinderClient();
            try
            {
                //m_Service.
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
