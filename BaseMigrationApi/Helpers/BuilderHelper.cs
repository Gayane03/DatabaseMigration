using BaseMigrationApi.DependencyInjection;
using BaseMigrationApi.Extensions;
using BusinessLayer.Helper;
using BusinessLayer.Mapping;

namespace BaseMigrationApi.Helper
{
	internal static class BuilderHelper
	{
		public static WebApplication BuildApplicationStructure(WebApplicationBuilder builder)
		{
			builder.Services.AddControllers();

			builder.Services.AddConnectionFront();//fronti jamank *******

			// Add services to the container.
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			//builder.Services.AddEndpointsApiExplorer(); // ogtagorcel erb swaggerov enq test anum******


			builder.Services.AddDependencies();

			builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

			builder.AddConfigurations();

			builder.AddLocalAuthentications();
			builder.AddLocalAuthorizations();

			//builder.Services.AddSwagger();//test swaggeri jamank******


			return builder.Build();
		}

		public static void UseApplicationOpportunities(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				////swaggeri jamanak****

				//app.UseSwagger();
				//app.UseSwaggerUI(c =>
				//{
				//	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
				//});
			}


            app.UseCors("AllowBlazorClient");//blazori jamanak****

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
			
			app.MapControllers();
			app.Run();
		}
	}
}
