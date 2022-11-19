using System.Net;
using System.Net.Mail;

namespace IdentityUyelikSistemi_DotNet6.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("domingo29@ethereal.email");
                mail.To.Add("domingo29@ethereal.email");
                mail.Subject = $"www.cengiz.com::Şifre yenileme";
                mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşagıdaki linke tıklayınız.</h2><hr/>";
                mail.Body += $"<a href='{link}'>Şifre yenileme linki</a>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.ethereal.email", 587))
                {
                    smtp.Credentials = new NetworkCredential("domingo29@ethereal.email", "FzFTDfaRDQPv4XQuzV");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }


            }
        }
    }
}
