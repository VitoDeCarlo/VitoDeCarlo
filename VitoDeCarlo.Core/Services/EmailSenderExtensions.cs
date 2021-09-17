using System.Text.Encodings.Web;

namespace VitoDeCarlo.Core.Services;

public static class EmailSenderExtensions
{
    private static readonly string standardFooter = "Thank you for using thePulse.com!";

    public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
    {
        var htmlBody = ReadTemplateFile("AuthMessage");
        var encodedUrl = HtmlEncoder.Default.Encode(link);

        htmlBody = htmlBody.Replace("{{HEADING}}", "Confirm your email");
        htmlBody = htmlBody.Replace("{{BODY}}", "A registration request to thePulse.com has been received.  To continue, " +
                $"please confirm your email address by <a href='{encodedUrl}'>clicking here</a>.");
        htmlBody = htmlBody.Replace("_tp__BUTTON_TEXT__tp_", "Confirm your email");
        htmlBody = htmlBody.Replace("_tp__BUTTON_URL__tp_", encodedUrl);
        htmlBody = htmlBody.Replace("{{FOOTER}}", standardFooter);

        return emailSender.SendEmailAsync(email, "Confirm your email", htmlBody);
    }

    public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
    {
        var htmlBody = ReadTemplateFile("AuthMessage");
        var encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);

        htmlBody = htmlBody.Replace("{{HEADING}}", "Reset Your Password");
        htmlBody = htmlBody.Replace("{{BODY}}", "A request to reset your password has been received.  To continue, " +
                $"please reset your password by <a href='{encodedUrl}'>clicking here</a>.");
        htmlBody = htmlBody.Replace("_tp__BUTTON_TEXT__tp_", "Reset My Password");
        htmlBody = htmlBody.Replace("_tp__BUTTON_URL__tp_", encodedUrl);
        htmlBody = htmlBody.Replace("{{FOOTER}}", standardFooter);

        return emailSender.SendEmailAsync(email, "Reset Password", htmlBody);
    }

    private static string ReadTemplateFile(string templateName)
    {
        var rootPath = Path.GetFullPath("~/");
        rootPath = rootPath.Substring(0, rootPath.IndexOf("~"));
        var fullPath = rootPath + @"wwwroot\templates\" + templateName + ".html";
        var htmlBody = File.ReadAllText(fullPath);
        return htmlBody;
    }
}