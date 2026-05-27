using System.Security.Authentication;
using Serilog;   
using inventory.GlobalException;
using inventory.Injection;
using DotNetEnv;
namespace inventory.Middleware;

class Configuration
{
    static public WebApplication webApplication()
    {
        var builder = WebApplication.CreateBuilder();
        Env.Load();
        builder.Configuration.AddEnvironmentVariables();
        Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();
        builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
        builder.Host.UseSerilog();
        var JWTKey = builder.Configuration["JWT_KEY"] ?? throw new InvalidCredentialException("");
        builder.Services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {  
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT_ISSUER"],    
                    ValidAudience = builder.Configuration["JWT_AUDIENCE"], 
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(JWTKey)) 
                };
            });
        builder.Services.AddApplicationServices(builder.Configuration);
        var app = builder.Build();
        return app;
    }
}