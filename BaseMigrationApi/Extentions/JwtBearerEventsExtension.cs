using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace BaseMigrationApi.Extentions
{
	public static class JwtBearerEventsExtension
	{
		public static void JwtBearerOptionsHelper(this JwtBearerOptions options, Func<string, ClaimsPrincipal?> prin)
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;

			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
					var principal = prin.Invoke(token);

					if (principal != null)
					{
						context.Principal = principal;
						context.Success();
					}
					return Task.CompletedTask;
				}
			};

			options.IncludeErrorDetails = true;
		}
	}
}
