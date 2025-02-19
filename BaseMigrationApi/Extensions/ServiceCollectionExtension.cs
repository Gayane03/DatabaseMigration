using Microsoft.OpenApi.Models;

namespace BaseMigrationApi.DependencyInjection
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddConnectionFront(this IServiceCollection services)
		{
			return services.AddCors(options =>
			{
				options.AddPolicy("AllowBlazorClient", builder =>
					builder.WithOrigins("https://localhost:7291")
						   .AllowAnyMethod()
						   .AllowAnyHeader().AllowCredentials());
			});
		}

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

				// User Access Token
				c.AddSecurityDefinition("UserAccessToken", new OpenApiSecurityScheme
				{
					Name = "UserAccessToken", // Distinct name
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					Description = "Enter your User Access Token in the format 'Bearer {token}'"
				});

				// Email Verification Token
				c.AddSecurityDefinition("EmailVerificationToken", new OpenApiSecurityScheme
				{
					Name = "EmailVerificationToken", // Distinct name
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					Description = "Enter your Email Verification Token in the format 'Bearer {token}'"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "UserAccessToken" // Reference the User Access Token
                                     }
						},
						new string[] {} //  Could be used for specific scopes if needed
                    },

					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "EmailVerificationToken" // Reference the Email Verification Token
                                }
						},
						new string[] {} // Could be used for specific scopes if needed
                    }
				});
			});

			return services;
		}

	}
}
