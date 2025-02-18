using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace UserAccessControl
{
	public interface IJwtTokenHandler
	{
		public TokenValidationParameters GetTokenValidationParameters();
		public string GenerateJwtToken(string userId, string userRole);
		public ClaimsPrincipal? ValidateJwtToken(string token);
	}
}
