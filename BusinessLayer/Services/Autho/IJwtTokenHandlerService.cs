using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Autho
{
	public interface IJwtTokenHandlerService
	{
		public TokenValidationParameters GetTokenValidationParameters();
		public string GenerateJwtToken(string userId, string userRole);
		public ClaimsPrincipal? ValidateJwtToken(string token);
	}
}
