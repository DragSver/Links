using Links.Core.Caching;
using Links.Core.RabbitMq;
using Links.Domain.ConfigureOptions;
using Links.UrlProcessor.Interfaces;
using Links.UrlProcessor.Options;
using Links.UrlProcessor.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Links.UrlProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
            .Build();

        host.Run();
    }

    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.Configure<RpcOptions>(hostContext.Configuration.GetSection("RpcServer"));
        services.Configure<UrlsOptions>(hostContext.Configuration.GetSection("Urls"));
        services.Configure<CacheOptions>(hostContext.Configuration.GetSection("Redis").GetSection("CacheKeys"));

        services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = hostContext.Configuration.GetSection("Redis")["ConnectionString"];
        });

        services.AddScoped<ICacheProvider, CacheProvider>();
        services.AddScoped<IRabbitService, RabbitService>();

        services.AddTransient<ILinkService, LinkService>();
        services.AddTransient<StatusCodeService>();
        services.AddTransient<IStatusCodeService, CachedStatusCodeService>();

        services.AddHttpClient();
        services.AddHostedService<Worker>();
    }
}