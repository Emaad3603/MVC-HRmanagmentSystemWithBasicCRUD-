using DemoDAL.Models;
using System.Net;
using System.Net.Mail;

namespace DemoPL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            // Email Server gmail 

            var client = new SmtpClient("smtp.gmail.com",587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("emaadabdelhady@gmail.com", "whzdpkpfbvbklzye");

            client.Send("emaadabdelhady@gmail.com", email.Recipients, email.Subject, email.Body);

        }
    }
}
