using BusinessLayer.Helper.ModelHelper;
using SharedLibrary.ResponseModels;
using SharedLibrary.RequestModels;

namespace BusinessLayer.Services
{
	public interface IUserService
	{
		Task<Result<EmailVerificationTokenResponse>> TryRegisterUser(RegistrationRequest registrationRequest);

		Task<Result<RegistrationResponse>> TryVerifyUserEmail(EmailVerificationCodeRequest verifyEmailRequest,string? token);

		Task<Result<LoginResponse>> LoginUser(LoginRequest loginRequest);

	}
}
