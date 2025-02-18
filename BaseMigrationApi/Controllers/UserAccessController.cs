using BaseMigrationApi.Helpers;
using BusinessLayer.Autho;
using BusinessLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseMigrationApi.Controllers
{
	
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = Role.Admin , AuthenticationSchemes = TokenSchemeType.UserAccess)]
	public class UserAccessController : ControllerBase
	{
		private readonly IJwtTokenHandlerService jwtTokenHandlerService;
		public UserAccessController(IJwtTokenHandlerService jwtTokenHandlerService)
		{

		   this.jwtTokenHandlerService = jwtTokenHandlerService;	
		}
	
		[HttpGet("test")]
		public bool Test(string token)
		{
			try
			{
				var a = jwtTokenHandlerService.ValidateJwtToken(token);

				return true;
			}
			catch (Exception)
			{

				return false;
			}
			
		}

	}
}