using BaseMigrationApi.Helpers;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.RequestModels;

namespace BaseMigrationApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService userService;

		public UserController(IUserService userService)
		{
			this.userService = userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
		{
			var result = await userService.TryRegisterUser(registrationRequest);

			if (!result.IsSuccess)
			{
				return BadRequest(result.Error);
			}

			return Ok(result.Value);
		}

		[Authorize(AuthenticationSchemes = TokenSchemeType.EmailVerification)]
		[HttpPost("verifyEmail")]
		public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationCodeRequest verifyEmailRequest)
		{
			var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		
			var result = await userService.TryVerifyUserEmail(verifyEmailRequest,token);

			if (!result.IsSuccess)
			{
				return BadRequest(result.Error);
			}

			return Ok(result.Value);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
		{
			var result = await userService.LoginUser(loginRequest);

			if (!result.IsSuccess)
			{
				return BadRequest(result.Error);
			}

			return Ok(result.Value);
		}
	}
}
