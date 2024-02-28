using Links.Core.RabbitMq;
using Links.Domain.ConfigureOptions;
using Links.UrlProcessor.Services;

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

        services.AddScoped<IRabbitService, RabbitService>();
        services.AddTransient<IUrlService, UrlService>();
        services.AddHttpClient();
        services.AddHostedService<Worker>();
    }
}