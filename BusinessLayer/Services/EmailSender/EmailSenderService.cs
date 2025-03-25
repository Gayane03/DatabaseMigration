using System.Net.Mail;
using System.Net;

namespace BusinessLayer.Services.EmailSender
{
	public class EmailSenderService: IEmailSenderService
	{
		public async Task SendEmailAsync(string toEmail, string verificationCode)
		{

			string fromEmail = "*********@gmail.com";
			string password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD",EnvironmentVariableTarget.User);

			try
			{
				using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
				{
					client.Port = 587; // Use port 587 for TLS
					client.Credentials = new NetworkCredential(fromEmail, password);
					client.EnableSsl = true; // Enable SSL/TLS
					client.DeliveryMethod = SmtpDeliveryMethod.Network;

					MailMessage mailMessage = new MailMessage
					{
						From = new MailAddress(fromEmail),
						Subject = "Email Verification Code",
						Body = $"Your verification code is: {verificationCode}",
						IsBodyHtml = false,
					};

					mailMessage.To.Add("********@gmail.com");

					await client.SendMailAsync(mailMessage);
				}
			}
			catch (Exception ex)
			{
				 
			}
			
		}
	}
}
