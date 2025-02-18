using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BusinessLayer.Autho
{
	public  abstract class BaseTokenService
	{
		public readonly IConfiguration config;
		public BaseTokenService(IConfiguration config)
		{
			this.config = config;
		}
		protected SigningCredentials GenerateKey()
		{
			var jwtKey = config["Jwt:Key"];
			if (string.IsNullOrWhiteSpace(jwtKey))
			{
				throw new InvalidOperationException("JWT key is missing or empty.");
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
			return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		}
	}
}
