using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Extensions;
using UrlShortener.UniqueUrlCodesGeneration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(connectionString));

builder.Services.AddSingleton<UrlShorteningService>();
builder.Services.AddSingleton<UniqueUrlCodeProvider>();
builder.Services.AddSingleton<UniqueUrlCodesPool>();
builder.Services.AddSingleton<UrlCodeUsedEvent>();
builder.Services.AddSingleton<UrlCodeGenerationSubscriber>();

var app = builder.Build();

// Trigger subscription
var subscriber = app.Services.GetRequiredService<UrlCodeGenerationSubscriber>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();

    app.UseCors(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
