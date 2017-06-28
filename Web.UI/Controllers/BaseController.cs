using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Web.UI.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public string CallbackUrl(string encryptedVerificationCode)
        {
            var routeValues = new {code = encryptedVerificationCode};
            var callbackUrl = Url.Action("ConfirmEmail", "Account", routeValues, protocol: Request.Url.Scheme);
            return callbackUrl;
        }

        public void SendEmail(string callBackUrl, string email)
        {
            var body = $"Please confirm your email address by clicking this <a href=\"{callBackUrl}\">link</a>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("***", email));
            message.From = new MailAddress("***");
            message.Subject = "Please Confirm your email address";
            message.Body = string.Format(body);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "***",
                    Password = "***"
                };
                smtp.Credentials = credential;
                smtp.Host = "***";
                smtp.Port = 000;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
        }
    }
}