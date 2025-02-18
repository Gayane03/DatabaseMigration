using AutoMapper;
using BusinessLayer.Autho;
using BusinessLayer.Helper;
using BusinessLayer.Helper.ModelHelper;
using BusinessLayer.Services.EmailSender;
using RepositoryLayer;
using SharedLibrary.DbModels.Request;
using SharedLibrary.RequestModels;
using SharedLibrary.ResponseModels;

namespace BusinessLayer.Services
{
	public class UserService : IUserService
	{
		private readonly IJwtTokenHandlerService jwtTokenHandlerService;
		private readonly IEmailVerificationTokenService emailVerificationTokenService;
		private readonly IEmailSenderService emailSenderService;
		private readonly IMapper mapper;
		private readonly IEmailVerificationService emailVerificationService;

		private readonly IRegistrationRepository registrationRepository;

		public UserService(
			IRegistrationRepository registrationRepository,
			IMapper mapper,
			IJwtTokenHandlerService jwtTokenHandlerService,
			IEmailVerificationTokenService emailVerificationTokenService,
			IEmailSenderService emailSenderService,
			IEmailVerificationService emailVerificationService)
		{
			this.registrationRepository = registrationRepository;
			this.mapper = mapper;
			this.jwtTokenHandlerService = jwtTokenHandlerService;
			this.emailVerificationTokenService = emailVerificationTokenService;
			this.emailSenderService = emailSenderService;
			this.emailVerificationService = emailVerificationService;
		}

		public async Task<Result<RegistrationResponse>> TryVerifyUserEmail(EmailVerificationCodeRequest verifyEmailRequest, string? token)
		{
			try
			{
				var verifiedUserId = emailVerificationService.TryVerifyUserEmail(verifyEmailRequest.VerificationCode, token);

				if (!verifiedUserId.IsSuccess)
				{
					return Result<RegistrationResponse>.Failure(verifiedUserId.Error);
				}

				var userId = Int32.Parse(verifiedUserId.Value);

				await registrationRepository.ChangeUserToActive(userId);

				var userAccessToken = jwtTokenHandlerService.GenerateJwtToken(verifiedUserId.Value, Role.Admin);

				return Result<RegistrationResponse>.Success(new(userAccessToken));
			}
			catch (Exception)
			{
				return Result<RegistrationResponse>.Failure(Message.SystemError);
			}
		}

		public async Task<Result<EmailVerificationTokenResponse>> TryRegisterUser(RegistrationRequest registrationRequest)
		{
			try
			{
				var userEmailRequest = mapper.Map<UserEmailRequest>(registrationRequest);

				var userActiveData = await registrationRepository.GetUserActivityData(userEmailRequest);
				if (userActiveData is not null)
				{
					if(userActiveData.IsActive)
					{
						return Result<EmailVerificationTokenResponse>.Failure(Message.EmailActivityError);
					}
					else
					{
						await registrationRepository.DeleteUserWithId(userActiveData.Id);
					}
				}


				var userData = mapper.Map<User>(registrationRequest);
				var userId = await registrationRepository.GenerateUser(userData);
				if (userId == default(int))
				{
					throw new Exception();
				}

				var emailVerificationToken = emailVerificationTokenService.GenerateVerificationToken(userId, out var verificationCode);
				await emailSenderService.SendEmailAsync(registrationRequest.Email, verificationCode);

				return Result<EmailVerificationTokenResponse>.Success(new EmailVerificationTokenResponse(emailVerificationToken));
			}
			catch (Exception ex)
			{
				// add logging process (ex.message)
				return Result<EmailVerificationTokenResponse>.Failure(Message.SystemError);
			}
		}

		public async Task<Result<LoginResponse>> LoginUser(LoginRequest loginRequest)
		{
			try
			{
				var mapRequest = mapper.Map<LoginRequestDB>(loginRequest);
				var loginUserModel = await registrationRepository.GetLoginProcessResponse(mapRequest);

				if (loginUserModel is null || loginUserModel.Id is default(int))
				{
					return Result<LoginResponse>.Failure(Message.LoginFailError);
				}

				var accessToken = jwtTokenHandlerService.GenerateJwtToken(loginUserModel.Id.ToString(), Role.Admin);

				return Result<LoginResponse>.Success(new(accessToken));
			}
			catch (Exception)
			{
				return Result<LoginResponse>.Failure(Message.SystemError);
			}
		}
	}
}
