using Css.Api.Core.EventBus.Services;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using GreenPipes;
using RabbitMQ.Client;
using MassTransit.RabbitMqTransport.Topology;

namespace Css.Api.Core.EventBus
{
    public static class BusRegistrations
    {
        public static void RegisterEventBus( this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBusService, BusService>();
            services.AddMassTransitHostedService();
        }

        public static void RegisterRabbitMQHost(this IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
        {
            cfg.Host(configuration.GetConnectionString("ServiceBus"));
            cfg.ExchangeType = ExchangeType.Topic;
            cfg.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(5)));
        }

        public static void RegisterExchange(this IRabbitMqReceiveEndpointConfigurator cfg, string exchange, string routingKey)
        {
            cfg.ConfigureConsumeTopology = false;
            cfg.Bind(exchange, e =>
            {
                e.RoutingKey = routingKey;
                e.ExchangeType = ExchangeType.Topic;
            });
            
        }

        public static void RegisterPublisher<T>(this IRabbitMqBusFactoryConfigurator cfg, string exchange)
            where T : class
        {
            cfg.Message<T>(c => c.SetEntityName(exchange));
            cfg.Publish<T>(c => { c.ExchangeType = ExchangeType.Topic; });
        }
    }
}
