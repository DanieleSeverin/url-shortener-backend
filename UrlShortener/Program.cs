using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Middlewares;
using UrlShortener.UniqueUrlCodesGeneration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole(); 
builder.Logging.AddDebug(); 
builder.Logging.AddAzureWebAppDiagnostics();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

//var connectionString = builder.Configuration.GetConnectionString("Database") ??
//            throw new ArgumentNullException(nameof(builder.Configuration));

var connectionString = System.Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_Database") ??
    builder.Configuration.GetConnectionString("POSTGRESQLCONNSTR_Database");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
});

builder.Services.AddSingleton<UrlShorteningService>();
builder.Services.AddSingleton<UniqueUrlCodeProvider>();
builder.Services.AddSingleton<UniqueUrlCodesPool>();
builder.Services.AddSingleton<UrlCodeUsedEvent>();
builder.Services.AddSingleton<UrlCodeGenerationSubscriber>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Trigger subscription
var subscriber = app.Services.GetRequiredService<UrlCodeGenerationSubscriber>();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

//app.ApplyMigrations();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
});

app.MapControllers();

app.Run();
