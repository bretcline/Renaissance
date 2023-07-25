using Jaxis.POS.Data;
using Jaxis.Utilities.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLoader
{
    public partial class Form1 : Form
    {
        public delegate void BarDelegate();
        ExcelLoaders.CSVBulkLoad m_Loader = null;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            m_Loader = new ExcelLoaders.CSVBulkLoad();

            cmbTableList.Items.AddRange(m_Loader.GetTableList().ToArray( ));

            var delimiters = new Dictionary<char, string>();
            delimiters.Add('|', "Pipe");
            delimiters.Add('\t', "Tab");
            delimiters.Add(',', "Comma");

            cmbDelimiter.DataSource = new BindingSource(delimiters, null);
            cmbDelimiter.DisplayMember = "Value";
            cmbDelimiter.ValueMember = "Key";

            var cultures = CultureInfo.GetCultures( CultureTypes.AllCultures );
            cmbCulture.DataSource = new BindingSource( cultures, null );
            cmbCulture.DisplayMember = "DisplayName";

            cmbCulture.SelectedItem = CultureInfo.CurrentCulture;

            cmbTableList.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 10;
            progressBar1.Minimum = 0;
            
            var config = System.Configuration.ConfigurationManager.AppSettings["ConfigFile"];
            var loader = new Micros.DataLoader.JournalLoader(config, false, "" );

            var path = System.Configuration.ConfigurationManager.AppSettings["JournalPath"];

            Action<string, Action> act = (a, b) => { loader.LoadJournal(a, b); };

            act.BeginInvoke(path, UpdateBar, null, null);

        }

        private void UpdateBar()
        {

            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { UpdateBar( ); });
            }
            else
            {
                if (progressBar1.Value == progressBar1.Maximum)
                {
                    progressBar1.Value = 0;
                }
                progressBar1.Value++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ofdDataFile.Filter = "Excel|*.xls;*.xlsx;";
            if( DialogResult.OK == ofdDataFile.ShowDialog() )
            {
                var sage = new ExcelLoaders.SageImport();
                sage.LoadSageData(ofdDataFile.FileName, new DateTime( 2015, 9, 1 ) );
            }

        }

        private void LoadOperaData( string tableName )
        {
            ofdDataFile.Filter = "Text|*.txt;*.csv;";
            if (DialogResult.OK == ofdDataFile.ShowDialog())
            {
                m_Loader.LoadData(tableName, ofdDataFile.FileName, '\t', 10000, "imp", 1 );
            }
        }

        private void btnOperaFourWeek_Click(object sender, EventArgs e)
        {
            LoadOperaData("OPERAFourWeekForecast");
        }

        private void btnOperaForecast_Click(object sender, EventArgs e)
        {
            LoadOperaData("OPERAResFutureOccupancy");
        }

        private void btnLoadMicros_Click(object sender, EventArgs e)
        {
            var selectedDelimiter = cmbDelimiter.SelectedItem as KeyValuePair<char, string>?;
            m_Loader.CultureToUse = cmbCulture.SelectedItem as CultureInfo;
//            ofdDataFile.Filter = "Text|*.txt;*.csv;";
//            if (DialogResult.OK == ofdDataFile.ShowDialog())
            {
                if (selectedDelimiter.HasValue && !string.IsNullOrWhiteSpace( txtFile.Text ) )
                {
                    m_Loader.LoadData(cmbTableList.Text, txtFile.Text, selectedDelimiter.Value.Key, 10000, "imp", 2, chkNoHeaders.Checked ? 0 : 1 );
                }
            }
        }

        private void btnFindFileClick (object sender, EventArgs e)
        {
            if( DialogResult.OK == ofdDataFile.ShowDialog() )
            {
                txtFile.Text = ofdDataFile.FileName;
            }            
        }

        private void btnWeather_Click(object sender, EventArgs e)
        {
            // 05b5076ddac68f83741aaeb31dce6516
            // latitude 43.525569 and longitude 5.438356

            var data = new WeatherData.WeatherData();
            data.LoadData("05b5076ddac68f83741aaeb31dce6516", 43.525569f, 5.438356f);
        }
    }
}
