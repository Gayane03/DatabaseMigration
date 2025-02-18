using BaseMigrationApi.Helper;

var builder = WebApplication.CreateBuilder(args);

var app = BuilderHelper.BuildApplicationStructure(builder);

BuilderHelper.UseApplicationOpportunities(app);
