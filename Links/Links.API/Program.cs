using Links.BL.Services;
using Links.BL.Workflow;
using Links.Core.RabbitMq;
using Links.DataAccess;
using Links.Domain.ConfigureOptions;
using Links.Domain.Interfaces;
using MediatR;
using System.Reflection;

namespace Links.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<QueueOptions>(builder.Configuration.GetSection("RPC"));
        builder.Services.Configure<RpcOptions>(builder.Configuration.GetSection("RPC").GetSection("Rabbit"));

        // Add services to the container.
        builder.Services.AddTransient<ILinksService, LinksService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.SetUpDatabase(builder.Configuration);

        builder.Services.AddScoped<IRabbitService, RabbitService>();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        builder.Services.AddMediatR(typeof(GetLinkRequest).GetTypeInfo().Assembly);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
