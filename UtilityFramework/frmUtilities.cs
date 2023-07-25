using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Guifreaks.NavigationBar;
using log4net;

namespace UtilityFramework
{
    public partial class frmUtilities : Form
    {
        Dictionary<Type, Form> m_ActiveForms = new Dictionary<Type, Form>();

        protected static ILog m_Logger = LogManager.GetLogger(typeof(frmUtilities));

        protected ConnectionStrings m_Conn;

        public enum ConnectionType
        {
            Prod,
            Test,
            Dev,
        }

        public frmUtilities()
        {
            InitializeComponent();
            var settings = new Properties.Settings();

            //m_Conn = new ConnectionStrings {SqlServer = settings.SqlConnection};
            SetConnectionString(ConnectionType.Prod);


            //var buttons = EagleFordUtils.GetUtilList(m_Logger, m_Conn);

            int count = 0;
            foreach( var fileImport in buttons )
            {
                AddButton(count++, fileImport);
            }
        }

        protected void AddButton( int position, IFileImport importer )
        {
            var button = new NaviButton
            {
                LargeImage = Properties.Resources.klipper_dock,
                Location = new Point( 0, position*47 ),
                Name = "nbtn" + importer.WindowName.Replace(" ", ""),
                Size = new Size( 145, 47 ),
                SmallImage = Properties.Resources.klipper_dock,
                TabIndex = 4,
                Text = importer.WindowName,
                Tag = importer
            };

            button.Click += ( sender, e ) => LaunchWindow(importer);


            naviBand1.ClientArea.Controls.Add(button);

        }

        private void LaunchWindow(IFileImport importer)
        {
            try
            {
                var frm = new frmSimpleData(importer) {Text = importer.WindowName, MdiParent = this};
                frm.Show();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void rdoProd_CheckedChanged(object sender, EventArgs e)
        {
            if( rdoProd.Checked )
            {
                SetConnectionString( ConnectionType.Prod );
            }
        }
        
        private void rdoTest_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTest.Checked)
            {
                SetConnectionString(ConnectionType.Test);
            }
        }

        private void rdoDev_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDev.Checked)
            {
                SetConnectionString(ConnectionType.Dev);
            }
        }
        
        private void SetConnectionString( ConnectionType prod )
        {
            try
            {
                switch( prod )
                {
                    case ConnectionType.Prod:
                    {
                        m_Conn.IDW = "Authentication Mechanism=LDAP;User Id=efacatdp;Data Source=HOUTDWP1;Password=Boat5673;Restrict to Default Database=True;Connection Timeout=0;";
                        //m_Conn.IDW = "Authentication Mechanism=LDAP;User Id=bcline;Data Source=HOUTDWP1;Password=Qwerty32;Restrict to Default Database=True;Connection Timeout=0;";
                        break;
                    }
                    case ConnectionType.Test:
                    {
                        m_Conn.IDW = "Authentication Mechanism=LDAP;User Id=efacatdt;Data Source=BVLTDWT1;Password=Wall7682;Restrict to Default Database=True;Connection Timeout=0;";
                        break;
                    }
                    case ConnectionType.Dev:
                    {
                        m_Conn.IDW = "Authentication Mechanism=LDAP;User Id=efacatdd;Data Source=HOUTDWD1;Password=Boat8135;Restrict to Default Database=True;Connection Timeout=0;";
                        break;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
