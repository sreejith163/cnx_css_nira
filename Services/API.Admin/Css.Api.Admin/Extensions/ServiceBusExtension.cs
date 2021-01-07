using Css.Api.Admin.EventHandlers.Registrations;
using Css.Api.Core.EventBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Css.Api.Admin.Extensions
{
    /// <summary>
    ///  A service extension class to configure the service bus
    /// </summary>
    public static class ServiceBusExtension
    {
        /// <summary>
        ///  Extension method to configure RabbitMQ service bus and the consumers using MassTransit
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEventBus(configuration);

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.RegisterRabbitMQHost(configuration);
                    cfg.RegisterEndPointReceivers(ctx);
                    cfg.RegisterPublishers();
                });

                config.RegisterBusConsumers();
            });

            return services;
        }
    }
}