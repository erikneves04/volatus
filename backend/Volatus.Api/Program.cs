using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Volatus.Api.Middleware;
using Volatus.Data.Context;
using Volatus.Data.Repositories;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.Services;
using Volatus.Domain.Interfaces.Services.Specials;
using Volatus.Domain.Services.Specials;
using System;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// services
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
services.AddScoped<IPermissionRepository, PermissionRepository>();
services.AddScoped<IDroneRepository, DroneRepository>();

services.AddScoped<IUserServices, UserServices>();
services.AddScoped<IPermissionServices, PermissionServices>();
services.AddScoped<IDroneServices, DroneServices>();
services.AddScoped<IAuthenticationServices, AuthenticationServices>();
services.AddScoped<ISessionServices, SessionServices>();
services.AddScoped<IPasswordServices, PasswordServices>();

// Add HttpContextAccessor
services.AddHttpContextAccessor();

services.AddControllers();

// Configure Swagger/OpenAPI
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

Console.WriteLine("Environment: " + environment.EnvironmentName);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Migrating database
    Console.WriteLine("Migrating database...");
    using var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
    Console.WriteLine("Database migrated successfully");
}

app.MapGet("/", () =>
{
    return Results.Ok("Volatus API is running!");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();

app.Run();