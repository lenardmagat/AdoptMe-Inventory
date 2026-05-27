using Microsoft.EntityFrameworkCore;
using inventory.Services;
using inventory.DataBase;
using inventory.CredentialSecurity;
using HashidsNet;

using Microsoft.IdentityModel.Protocols.Configuration;
namespace inventory.Injection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DATABASE_CONNECTION"];
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("DATABASE_CONNECTION environment variable is missing!");
        }
        services.AddDbContext<DbManager>(options => options.UseNpgsql(connectionString));
        services.AddSingleton<IHashids>(_ => new Hashids(configuration["HASHIDS_SALT"], 5));
        services.AddSingleton<IHasher>(sp =>
        {
            var hashids = sp.GetRequiredService<IHashids>();
            string? keystring = configuration["JWT_KEY"] ?? throw new InvalidConfigurationException("JWT key string is missing.");
            string? issuer = configuration["JWT_ISSUER"] ?? throw new InvalidConfigurationException("Issuer key string is missing.");
            string? audience = configuration["JWT_AUDIENCE"] ?? throw new InvalidConfigurationException("Audience key string is missing.");
            return new Security(hashids, keystring, issuer, audience);
        }
        );
        services.Scan(scan => scan
        .FromAssemblyOf<RegisterService>()
        .AddClasses(classes => classes.Where(t => t.Namespace != null && t.Namespace.Contains("Services")))
        .AsSelfWithInterfaces()
        .WithScopedLifetime()
        );
        services.AddScoped<AdvancedBufferService>(options => { return new AdvancedBufferService("logs/inventory_buffer.json");});
        return services;
    }
}