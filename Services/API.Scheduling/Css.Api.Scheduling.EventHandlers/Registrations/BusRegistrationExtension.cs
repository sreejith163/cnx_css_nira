using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Events.Client;
using Css.Api.Scheduling.EventHandlers.Consumers;
using Css.Api.Scheduling.EventHandlers.Faults;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Topology.Topologies;
using RabbitMQ.Client;

namespace Css.Api.Scheduling.EventHandlers.Registrations
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
            config.AddConsumer<CreateClientCommandConsumer>();
            config.AddConsumer<CreateClientCommandFault>();
        }

        /// <summary>
        ///  The extension method to register all endpoint receivers and their respective consumers in the service
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="ctx"></param>
        public static void RegisterEndPointReceivers(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {
            cfg.RegisterCreateClientCommandQueue(ctx);
        }

        /// <summary>
        /// The extension method to register all publisher with respective exchanges and queues
        /// </summary>
        /// <param name="cfg"></param>
        public static void RegisterPublishers(this IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.RegisterPublisher<IClientCreateSuccess>(MassTransitConstants.ClientExchange);
            cfg.RegisterPublisher<IClientCreateFailed>(MassTransitConstants.ClientExchange);
        }

        /// <summary>
        ///  The extension method for registering the ClientCommandQueue endpoint receiver
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="ctx"></param>
        private static void RegisterCreateClientCommandQueue(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {   
            cfg.ReceiveEndpoint(MassTransitConstants.ClientCommandQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.ClientExchange, MassTransitConstants.ClientCommandQueueBindingPattern);
                c.Consumer<CreateClientCommandConsumer>(ctx);
            });
            
        }
    }
}
