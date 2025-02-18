using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Autho
{
	public class EmailVerificationTokenService : BaseTokenService, IEmailVerificationTokenService
	{
		public EmailVerificationTokenService(IConfiguration config) : base(config) { }

		public string GenerateVerificationToken(int userId, out string verificationCode)
		{
			var id =  userId.ToString();	
			var random = new Random();
			verificationCode = random.Next(100000, 999999).ToString(); // 6-digit code

			var credentials = GenerateKey();

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, id),
			    new Claim("verification_code", verificationCode),
			    //new Claim("exp", DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds().ToString()) // Expiry in 10 min
            };

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(10),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


		public TokenValidationParameters GetEmailVerificationTokenValidationParameters()
		{
			
			var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);

			return new TokenValidationParameters
			{
				ValidateIssuer = false, // Set to true if you have an issuer
				ValidateAudience = false, // Set to true if you have an audience
				ValidateLifetime = true, // Ensure the token is not expired
				RequireExpirationTime = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ClockSkew = TimeSpan.Zero // Reduce clock skew to enforce strict expiration
			};
		}

		public ClaimsPrincipal ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = GetEmailVerificationTokenValidationParameters();

			try
			{
				return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
			}			
			catch (Exception ex)
			{
				// Catch any other exception that might occur.
				//Console.WriteLine($"General error during token validation: {ex.Message}");
				return null;
			}
		}

		public (string,string) GetVerificationDataFromToken(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			var verificationCodeClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "verification_code");
			var idClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

			return (verificationCodeClaim?.Value,idClaim); 
		}
	}

}