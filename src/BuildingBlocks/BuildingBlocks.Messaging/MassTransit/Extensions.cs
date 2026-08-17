using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    /// <summary>
    /// Only pass <paramref name="assembly"/> if it's publisher not a consumer
    /// </summary>
    /// <param name="configuration">the configuration of the service</param>
    /// <param name="assembly">assembly of the targeted service</param>
    /// <returns></returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration,
        Assembly? assembly = null)
    {
        //Implement rabbitmq masstransit configuration
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();
            if (assembly != null)
                config.AddConsumers(assembly);
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:Username"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        // Configure MassTransit with the provided options

        return services;
    }
}