using BaseMigrationApi.Helpers;
using BusinessLayer.Autho;
using BusinessLayer.Helper;
using Microsoft.AspNetCore.Authentication;

namespace BaseMigrationApi.Extentions
{
	public static class WebAppBuilderExtension
	{
		public static IConfigurationBuilder AddConfigurations(this WebApplicationBuilder builder)
		{
			return builder.Configuration
						   .AddJsonFile(StaticFileType.AppSettings, optional: true, reloadOnChange: true)
						   .AddJsonFile(StaticFileType.JwtSettings, optional: true, reloadOnChange: true);
		}

		public static IServiceCollection AddLocalAuthorizations(this WebApplicationBuilder builder)
		{
			return builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy(Role.Admin, policy =>
				{
					policy.RequireAuthenticatedUser()
						  .RequireRole(Role.Admin)
						  .AddAuthenticationSchemes(TokenSchemeType.UserAccess);
				});
			});
		}

		public static AuthenticationBuilder AddLocalAuthentications(this WebApplicationBuilder builder)
		{
			var authenticationBuilder = builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = TokenSchemeType.UserAccess;
				options.DefaultChallengeScheme = TokenSchemeType.UserAccess;
			})
			.AddJwtBearer(TokenSchemeType.UserAccess, options =>
			{
				options.JwtBearerOptionsHelper(new JwtTokenHandlerService(builder.Configuration).ValidateJwtToken);
			})
			.AddJwtBearer(TokenSchemeType.EmailVerification, options =>
			{
				options.JwtBearerOptionsHelper(new EmailVerificationTokenService(builder.Configuration).ValidateToken);
			});

			return authenticationBuilder;
		}

	}
}
