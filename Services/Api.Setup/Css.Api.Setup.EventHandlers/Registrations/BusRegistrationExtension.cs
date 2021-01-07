using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.Client;
using Css.Api.Setup.EventHandlers.Consumers;
using Css.Api.Setup.EventHandlers.Faults;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Setup.EventHandlers.Registrations
{
    /// <summary>
    /// A bus registeration extension class which contains all service bus related configurations and registrations
    /// </summary>
    public static class BusRegistrationExtension
    {
        /// <summary>
        /// The extension method to register all consumers in the service
        /// </summary>
        /// <param name="config"></param>
        public static void RegisterBusConsumers(this IServiceCollectionBusConfigurator config)
        {
            config.AddConsumer<ClientCreateSuccessConsumer>();
            config.AddConsumer<ClientCreateSuccessFault>();

            config.AddConsumer<ClientCreateFailedConsumer>();
            config.AddConsumer<ClientCreateFailedFault>();
        }

        /// <summary>
        ///  The extension method to register all endpoint receivers and their respective consumers in the service
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="ctx"></param>
        public static void RegisterEndPointReceivers(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {
            cfg.RegisterClientEventQueueReceiver(ctx);
        }

        /// <summary>
        /// The extension method to register all publisher with respective exchanges and queues
        /// </summary>
        /// <param name="cfg"></param>
        public static void RegisterPublishers(this IRabbitMqBusFactoryConfigurator cfg)
        {
            //TODO register all publisher using the RegisterPublisher<T>(exchangeName, queueName)
            cfg.RegisterPublisher<CreateClientCommand>(MassTransitConstants.ClientExchange);
        }

        /// <summary>
        ///  The extension method for registering the ClientEventQueue endpoint receiver
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="ctx"></param>
        private static void RegisterClientEventQueueReceiver(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {
            cfg.ReceiveEndpoint(MassTransitConstants.ClientEventQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.ClientExchange, MassTransitConstants.ClientEventQueueBindingPattern);
                c.Consumer<ClientCreateSuccessConsumer>(ctx);
                c.Consumer<ClientCreateFailedConsumer>(ctx);
            });

        }
    }
}
