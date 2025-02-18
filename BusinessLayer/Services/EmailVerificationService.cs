using BusinessLayer.Autho;
using BusinessLayer.Helper;
using BusinessLayer.Helper.ModelHelper;

namespace BusinessLayer.Services
{
	public class EmailVerificationService : IEmailVerificationService
	{
		private readonly IEmailVerificationTokenService emailVerificationTokenService;

		public EmailVerificationService(IEmailVerificationTokenService emailVerificationTokenService)
		{
			this.emailVerificationTokenService = emailVerificationTokenService;
		}

		public Result<string> TryVerifyUserEmail(int currentVerificationCode, string? token )
		{
			try
			{
				var (verificationCode, userId) = emailVerificationTokenService.GetVerificationDataFromToken(token);

				if (verificationCode != currentVerificationCode.ToString())
				{
					return Result<string>.Failure(Message.VerificationCodeIncorrected);
				}

				return Result<string>.Success(userId);
			}
			catch (Exception ex)
			{
				return Result<string>.Failure(Message.SystemError);
			}
		}
	}
}
