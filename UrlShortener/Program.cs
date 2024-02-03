using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Extensions;
using UrlShortener.UniqueUrlCodesGeneration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(builder.Configuration)); ;

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

app.UseSwagger();
app.UseSwaggerUI();

app.ApplyMigrations();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
});


app.MapControllers();

app.Run();
