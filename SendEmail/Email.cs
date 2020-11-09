using System.Net;
using System.Net.Mail;

namespace SendEmail
{
    public class Email
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public void Send()
        {
            new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("4abdc5759b5406", "4512aa085f7e3c"),
                EnableSsl = true
            }.Send("schedule_noreplay@gmail.com", To, Subject, Body);
        }
    }
}
