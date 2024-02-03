using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Extensions;
using UrlShortener.Middlewares;
using UrlShortener.UniqueUrlCodesGeneration;
using Microsoft.Extensions.Logging.AzureAppServices;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug(); 
builder.Logging.AddAzureWebAppDiagnostics();

Logger.Log("Start");

builder.Services.AddControllers();
Logger.Log("AddControllers");
builder.Services.AddEndpointsApiExplorer();
Logger.Log("AddEndpointsApiExplorer");
builder.Services.AddSwaggerGen();
Logger.Log("AddSwaggerGen");

builder.Configuration.AddEnvironmentVariables();
Logger.Log("AddEnvironmentVariables");

var connectionString = builder.Configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(builder.Configuration));

Logger.Log("GetConnectionString");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
});

Logger.Log("AddDbContext");

builder.Services.AddSingleton<UrlShorteningService>();
builder.Services.AddSingleton<UniqueUrlCodeProvider>();
builder.Services.AddSingleton<UniqueUrlCodesPool>();
builder.Services.AddSingleton<UrlCodeUsedEvent>();
builder.Services.AddSingleton<UrlCodeGenerationSubscriber>();

Logger.Log("AddSingleton");

var app = builder.Build();

Logger.Log("Build");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Logger.Log("SetSwitch");

// Trigger subscription
var subscriber = app.Services.GetRequiredService<UrlCodeGenerationSubscriber>();

Logger.Log("subscriber");

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlingMiddleware>();

Logger.Log("UseMiddleware");

app.UseSwagger();
app.UseSwaggerUI();

Logger.Log("UseSwagger");

app.ApplyMigrations();

Logger.Log("ApplyMigrations");

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
});

Logger.Log("UseCors");

app.MapControllers();

Logger.Log("MapControllers");

app.Run();
