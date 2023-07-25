using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    class ReportPublisher
    {

        public void SendReportViaEmail( string _report, string _summary )
        {
            try
            {
                var mail = new MailMessage();
                var smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("bret@jaxissolutions.com");
                mail.To.Add("bret.cline@renaissance-aix.com");
                mail.Subject = $"POS Daily Report - {DateTime.Today.ToShortDateString()}";
                mail.IsBodyHtml = true;
                mail.Body = $"<font size=11 face=\"Courier New\">{_summary}</font>"; ;
                
                var attachment = new System.Net.Mail.Attachment(_report);
                mail.Attachments.Add(attachment);

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("bret@jaxissolutions.com", "x10rocket");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
