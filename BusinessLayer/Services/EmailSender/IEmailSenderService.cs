

namespace BusinessLayer.Services.EmailSender
{
	public interface IEmailSenderService
	{
		public Task SendEmailAsync(string toEmail, string verificationCode);
	}
}
