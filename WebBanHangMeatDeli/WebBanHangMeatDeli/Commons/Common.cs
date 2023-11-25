using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace WebBanHangMeatDeli.Commons
{
    public class Common
    {

        public static void SendMail( string ToEmailAddress, string subject, string content )
        {
            var FromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var FromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var FromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var SMTPHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var SMTPPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            bool EnableSSL = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());
            string body = content;
            MailMessage message = new MailMessage(new MailAddress(FromEmailAddress, FromEmailDisplayName), new MailAddress(ToEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            var client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(FromEmailAddress, FromEmailPassword);
            client.Host = SMTPHost;
            client.EnableSsl = EnableSSL;
            client.Port = !string.IsNullOrEmpty(SMTPPort) ? Convert.ToInt32(SMTPPort) : 0;
            client.Send(message);
            //try
            //{
            //    MailMessage message = new MailMessage();
            //    var smtp = new SmtpClient();
            //    {
            //        smtp.Host = "smtp.gmail.com"; //host name
            //        smtp.Port = 587; //port number
            //        smtp.EnableSsl = true; //whether your smtp server requires SSL
            //        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            //        smtp.UseDefaultCredentials = false;
            //        smtp.Credentials = new NetworkCredential()
            //        {
            //            UserName = Email,
            //            Password = password
            //        };
            //    }
            //    MailAddress fromAddress = new MailAddress(Email, name);
            //    message.From = fromAddress;
            //    message.To.Add(toMail);
            //    message.Subject = subject;
            //    message.IsBodyHtml = true;
            //    message.Body = content;
            //    smtp.Send(message);
            //    rs = true;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    rs = false;
            //}
            //return rs;
        }
        public static string FormatNumber( object value, int SoSauDauPhay = 2 )
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }
            string str = "";
            string thapPhan = "";
            for (int i = 0; i < SoSauDauPhay; i++)
            {
                thapPhan += "#";
            }
            if (thapPhan.Length > 0) thapPhan = "." + thapPhan;
            string snumformat = string.Format("0:#,##0{0}", thapPhan);
            str = String.Format("{" + snumformat + "}", GT);

            return str;
        }
        private static bool IsNumeric( object value )
        {
            return value is sbyte
                       || value is byte
                       || value is short
                       || value is ushort
                       || value is int
                       || value is uint
                       || value is long
                       || value is ulong
                       || value is float
                       || value is double
                       || value is decimal;
        }
    }
}