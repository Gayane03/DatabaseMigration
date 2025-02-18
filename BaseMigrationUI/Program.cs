using BaseMigrationUI;
using BaseMigrationUI.EntryPoints;
using BaseMigrationUI.Helper;
using BaseMigrationUI.Helper.Validators;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

try
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	builder.RootComponents.Add<App>("#app");

	builder.RootComponents.Add<HeadOutlet>("head::after");

	builder.Services.AddScoped(sp =>
		new HttpClient { BaseAddress = new Uri("https://localhost:7230/api/") });

	builder.Services.AddTransient<RegistrationRequestValidator>();
	builder.Services.AddScoped<ResponseMessageUtile>();
	builder.Services.AddScoped<ApiController>();
	builder.Services.AddTransient<VerifyEmailRequestValidator>();
	builder.Services.AddTransient<LoginRequestValidator>();

	
	builder.Services.AddScoped<LocalStorageHelper>();
	//builder.Services.AddScoped<IJSRuntime>();
	//builder.Services.AddScoped<NavigationManager>();

	builder.Services.AddMudServices(config =>
	{
		config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
		config.SnackbarConfiguration.PreventDuplicates = false;
		config.SnackbarConfiguration.NewestOnTop = false;
		config.SnackbarConfiguration.ShowCloseIcon = true;
		config.SnackbarConfiguration.VisibleStateDuration = 10000;
		config.SnackbarConfiguration.HideTransitionDuration = 500;
		config.SnackbarConfiguration.ShowTransitionDuration = 500;
		config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
	});


	builder.Services.AddLocalization();
	await builder.Build().RunAsync();

}
catch (Exception)
{
}
