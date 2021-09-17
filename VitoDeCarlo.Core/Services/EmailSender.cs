using System.Net;
using System.Net.Mail;

namespace VitoDeCarlo.Core.Services;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var from = new MailAddress("message@thepulse.com", "thePulse Message");
        var to = new MailAddress(email);

        var msg = new MailMessage(from, to)
        {
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        var smtpClient = new SmtpClient
        {
            Host = "smtp.office365.com",
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential("message@thepulse.com", "3Fkm^5PLUm$EZMQ2")
        };

        return smtpClient.SendMailAsync(msg);
        //EmailService svc = new EmailService();
        //return svc.SendEmailAsync(new MailAddress("message@thePulse.com", "thePulse Message"), new MailAddress(email), subject, message);
    }
}