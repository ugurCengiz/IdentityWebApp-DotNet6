using System.Net;
using System.Net.Mail;

namespace IdentityUyelikSistemi_DotNet6.Helper
{
    public static class EmailConfirmation
    {
        public static void SendEmail(string link,string email)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("karson58@ethereal.email");
                mail.To.Add(email);   //"(karson58@ethereal.email");
                mail.Subject = $"www.cengiz.com::Email Doğrulama";
                mail.Body = "<h2>Mailinizi doğrulamak için lütfen aşagıdaki linke tıklayınız.</h2><hr/>";
                mail.Body += $"<a href='{link}'>Email Doğrulama linki</a>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.ethereal.email", 587))
                {
                    smtp.Credentials = new NetworkCredential("karson58@ethereal.email", "fyb62QY2PWEFFd6a9e");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }


            }
        }
    }
}
