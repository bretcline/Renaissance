using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicketFinder.svcTicketFinder;

namespace TicketFinder
{
    public partial class frmTicketFinder : Form
    {
        private Guid SessionID = new Guid("00000000-1111-2222-3333-000000000000");
        svcTicketFinder.TicketFinderClient m_Service = null;
        public frmTicketFinder()
        {
            InitializeComponent();
            dtpStartDate.Value = DateTime.Now.AddDays(-1);
            chkSingleDay.Checked = true;

            m_Service = new TicketFinder.svcTicketFinder.TicketFinderClient();

            var establishments = m_Service.GetEstablisments(SessionID).Select(p => p.Establishment).ToArray();
            cmbEstablishment.Items.AddRange(establishments);

            var timePeriods = m_Service.GetTimePeriods(SessionID);
            cmbTimePeriod.DisplayMember = "TimePeriodName";
            cmbTimePeriod.ValueMember = "TimePeriodID";
            cmbTimePeriod.DataSource = timePeriods;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DateTime? startDate = null;
                DateTime? endDate = null;
                int timePeriod = -1;

                if (dtpEndDate.Enabled)
                {
                    endDate = dtpEndDate.Value.Date;
                }
                if (dtpStartDate.Enabled)
                {
                    startDate = dtpStartDate.Value.Date;
                    if (chkSingleDay.Checked)
                    {
                        endDate = startDate.Value.AddDays(1);
                    }
                }
                if (chkTimePeriod.Checked)
                {
                    timePeriod = (int)cmbTimePeriod.SelectedValue;
                }

                var tickets = m_Service.GetTicketData(SessionID, txtTicketNumber.Text, startDate, endDate, chkEstablishment.Checked ? cmbEstablishment.Text : string.Empty, txtTicketText.Text, timePeriod);

                var ticketList = new SortableBindingList<DatavwPOSTicket>(tickets);
                ticketList.AfterSort = ColorRows;
                dgvData.DataSource = ticketList;


                var enumerator = dgvData.Columns.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var column = enumerator.Current as DataGridViewColumn;
                    if (null != column && column.Name.EndsWith("ID"))
                    {
                        column.Visible = false;
                    }
                    if (column.ValueType == typeof(decimal) || column.ValueType == typeof(decimal?))
                    {
                        column.DefaultCellStyle.Format = "c";
                    }
                    if (column.ValueType == typeof(DateTime) || column.ValueType == typeof(DateTime?))
                    {
                        column.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                }
                ColorRows();

                RestoreGridColumns();

                txtSummaryData.Text = GetSummaryData(tickets);
            }
            catch (Exception err)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(err.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void ColorRows()
        {
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (Convert.ToInt32(row.Cells["DiscountTotal"].Value) != 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Pink;
                }
                else if (Convert.ToInt64(row.Cells["TransactionType"].Value) == 1)
                {
                    row.DefaultCellStyle.BackColor = Color.PaleVioletRed;
                }
                else if (Convert.ToInt64(row.Cells["TransactionType"].Value) == 2)
                {
                    row.DefaultCellStyle.BackColor = Color.PaleTurquoise;
                }
                else if (Convert.ToInt64(row.Cells["TransactionType"].Value) == 3)
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private string GetSummaryData(List<DatavwPOSTicket> tickets)
        {
            var rc = new StringBuilder();
            try
            {
                var spacer = "\t";
                rc.Append(string.Format("Ticket Count:       {0}{1}", tickets.Count.ToString( ).PadLeft( 10, ' ' ), spacer));
                rc.Append(string.Format("Total Guests:       {0}{1}", tickets.Select(t => t.GuestCountModified).Sum().Value.ToString().PadLeft(10, ' '), Environment.NewLine));
                rc.Append(string.Format("Minimum Ticket:    ${0}{1}", Math.Round(tickets.Where(t => t.TicketTotal > 0.0M).Select(t => t.TicketTotal).Min().Value, 2).ToString().PadLeft(10, ' '), spacer));
                rc.Append(string.Format("Maximum Ticket:    ${0}{1}", Math.Round(tickets.Select(t => t.TicketTotal).Max().Value, 2).ToString().PadLeft(10, ' '), Environment.NewLine));
                rc.Append(string.Format("Sum of Discounts:  ${0}{1}", Math.Round(tickets.Select(t => t.DiscountTotal).Sum().Value, 2).ToString().PadLeft(10, ' '), Environment.NewLine));
                rc.Append(string.Format("Minibar Lost:      ${0}{1}", Math.Round(tickets.Where(t => t.TransactionType == 1).Select(t => Math.Abs((t.TicketTotal.HasValue) ? t.TicketTotal.Value : 0.0M)).Sum(), 2).ToString().PadLeft(10, ' '), spacer));
                rc.Append(string.Format("Amenities:         ${0}{1}", Math.Round(tickets.Where(t => t.TransactionType == 2).Select(t => Math.Abs( ( t.TicketTotal.HasValue ) ? t.TicketTotal.Value : 0.0M )).Sum(), 2).ToString().PadLeft(10, ' '), Environment.NewLine));
                rc.Append(string.Format("Sum of Tickets:    ${0}{1}", Math.Round(tickets.Select(t => (t.TicketTotal.HasValue) ? t.TicketTotal.Value : 0.0M).Sum(), 2).ToString().PadLeft(10, ' '), spacer));
                rc.Append(string.Format("Sum of Tips   :    ${0}{1}", Math.Round(tickets.Select(t => Math.Abs((t.TipAmount.HasValue) ? t.TipAmount.Value : 0.0M)).Sum(), 2).ToString().PadLeft(10, ' '), Environment.NewLine));
                rc.Append(string.Format("Sum of Credits:    ${0}{1}", Math.Round(tickets.Select(t => (t.TicketTotal.HasValue && t.TicketTotal.Value < 0.0M) ? t.TicketTotal.Value : 0.0M).Sum(), 2).ToString().PadLeft(10, ' '), spacer));
                rc.Append(string.Format("Sum of Payments:   ${0}{1}", Math.Round(tickets.Select(t => t.PaymentTotal).Sum().Value, 2).ToString().PadLeft(10, ' '), Environment.NewLine));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            return rc.ToString();
        }

        private void chkStartDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpStartDate.Enabled = chkStartDate.Checked;
        }

        private void chkEndDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpEndDate.Enabled = chkEndDate.Checked;
        }

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            var value = dgvData.SelectedRows[0].Cells["RawData"].Value.ToString();
            var ticketWindow = new frmTicketInfo(value);
            ticketWindow.Show();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void chkEstablishment_CheckedChanged(object sender, EventArgs e)
        {
            cmbEstablishment.Enabled = chkEstablishment.Checked;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //if (DialogResult.OK == sfdExport.ShowDialog())
            //{
            //    var export = new ExcelLoaders.ExcelExport<DatavwPOSTicket>();
            //    export.DataList = dgvData.DataSource as SortableBindingList<DatavwPOSTicket>;
            //    export.ExportData(sfdExport.FileName);
            //}
        }



        // Save order
        private void SaveGridColumns()
        {
            DataTable dt = new DataTable("GridColumnOrder");

            var query = from DataGridViewColumn col in dgvData.Columns
                        orderby col.DisplayIndex
                        select col;

            foreach (DataGridViewColumn col in query)
            {
                dt.Columns.Add(col.Name);
            }

            dt.WriteXmlSchema(Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TicketFinderGridOrder.xml"));
        }

        // Restore order
        private void RestoreGridColumns()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "TicketFinderGridOrder.xml");
            if (File.Exists(filePath))
            {
                DataTable dt = new DataTable();
                dt.ReadXmlSchema(filePath);

                int i = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    if (dgvData.Columns.Contains(col.ColumnName))
                    {
                        dgvData.Columns[col.ColumnName].DisplayIndex = i;
                        i++;
                    }
                }
            }
        }

        private void btnSaveColumns_Click(object sender, EventArgs e)
        {
            SaveGridColumns();
        }

        private void frmTicketFinder_Load(object sender, EventArgs e)
        {
            var windowsId = WindowsIdentity.GetCurrent();
            if (windowsId != null)
            {

            }
            else
            {

            }
        }

        private void chkTimePeriod_CheckedChanged(object sender, EventArgs e)
        {
            cmbTimePeriod.Enabled = chkTimePeriod.Checked;
        }

        private void btnCustomReports_Click(object sender, EventArgs e)
        {

        }

        private void btnTextFile_Click(object sender, EventArgs e)
        {

        }

        private void chkSingleDay_CheckedChanged(object sender, EventArgs e)
        {
            chkEndDate.Checked = !chkSingleDay.Checked;
            //dtpEndDate.Enabled = !chkSingleDay.Checked;
        }
    }
}
