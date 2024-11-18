using DotnetMongo.Infrastructure.Settings;
using DotnetMongo.Application.Modules;
using DotnetMongo.Infrastructure.Modules;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configure services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
ConfigureMiddleware(app);

app.Run();

return;
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();

    // Enable XML comments
    services.AddControllers()
        .AddControllersAsServices();

    services.AddEndpointsApiExplorer();

    ConfigureSwagger(services);

    ConfigureDatabase(services, configuration);

    RegisterApplicationServices(services);
}
void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerGen();
}
void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    // 1. Using Db Context and Mongodb configuration settings
    services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDbConfiguration"));

    //// 2. Without Db Context and mongodb configuration settings
    //var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));
    //services.AddSingleton<IMongoClient>(mongoClient);
}
void RegisterApplicationServices(IServiceCollection services)
{
    services.AddApplicationDependencies().AddInfrastructureDependencies();
}
void ConfigureMiddleware(WebApplication application)
{
    // Enable middleware to serve generated Swagger as a JSON endpoint and the Swagger UI
    application.UseSwagger();
    application.UseSwaggerUI();
    application.UseHttpsRedirection();
    application.UseRouting();
    application.UseAuthentication();
    application.UseAuthorization();
    application.MapControllers();
}