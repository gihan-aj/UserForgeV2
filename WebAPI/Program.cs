using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPI.Extensions;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.ConfigureAppSettings(configuration);

builder.Services.AddInfrastructure(configuration);

builder.Services.AddApplication();

builder.Services.AddAuthWithJwt(configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApiVersioningWithSwagger();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.ConfigureSwagger();
    app.ApplyMigrations();
}

app.ConfigureCORS(configuration);

app.UseAuth();

await app.SeedInitialDataAsync();

app.Run();