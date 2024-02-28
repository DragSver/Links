using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Links.DataAccess;

public static class DataAccessExtensions
{
    public static IServiceCollection SetUpDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionString"];
        var timeout = (int)TimeSpan.FromMinutes(60).TotalSeconds;

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, opts => opts.CommandTimeout(timeout));
            options.EnableSensitiveDataLogging();
        });

        return services;
    }
}
