using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketFinder
{
    public partial class frmTicketInfo : Form
    {
        public frmTicketInfo( string ticket)
        {
            InitializeComponent();
            txtTicket.Text = ticket;
        }

        private void frmTicketInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(this.txtTicket.Text, this.txtTicket.Font, Brushes.Black, 10, 25);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocumentOnPrintPage;

            if (chkDefaultPrinter.Checked)
            {
                printDocument.Print();
            }
            else
            {
                PrintDialog printdlg = new PrintDialog();
                //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();

                // preview the assigned document or you can create a different previewButton for it
                //printPrvDlg.Document = printDocument;
                //printPrvDlg.ShowDialog(); // this shows the preview and then show the Printer Dlg below

                printdlg.Document = printDocument;

                if (printdlg.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
        }
    }
}
