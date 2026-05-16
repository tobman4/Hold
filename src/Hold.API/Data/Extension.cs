using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hold.API.Data;

public static class Extension
{
    public static IServiceCollection AddHoldDb(this IServiceCollection services, IConfiguration configuration)
    {
        var dbPath = configuration["DB"];
        if (string.IsNullOrEmpty(dbPath))
        {
            throw new ArgumentNullException(nameof(dbPath), "Database path not configured in 'DB' setting.");
        }

        services.AddDbContext<HoldDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        return services;
    }
}
