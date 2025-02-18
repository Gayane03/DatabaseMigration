using BusinessLayer.Helper.ModelHelper;

namespace BusinessLayer.Services
{
	public interface IEmailVerificationService
	{
		Result<string> TryVerifyUserEmail(int verificationCode, string? token);
	}
}
