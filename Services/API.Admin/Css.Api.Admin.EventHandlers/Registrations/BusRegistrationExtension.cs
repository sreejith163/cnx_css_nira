using Css.Api.Admin.EventHandlers.Consumers.AgentCategory;
using Css.Api.Admin.EventHandlers.Consumers.SchedulingCode;
using Css.Api.Admin.EventHandlers.Faults.AgentCategory;
using Css.Api.Admin.EventHandlers.Faults.SchedulingCode;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentCategory;
using Css.Api.Core.EventBus.Commands.SchedulingCode;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;

namespace Css.Api.Admin.EventHandlers.Registrations
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
            config.AddConsumer<SchedulingCodeCreateSuccessConsumer>();
            config.AddConsumer<SchedulingCodeCreateSuccessFault>();

            config.AddConsumer<SchedulingCodeCreateFailedConsumer>();
            config.AddConsumer<SchedulingCodeCreateFailedFault>();

            config.AddConsumer<SchedulingCodeUpdateSuccessConsumer>();
            config.AddConsumer<SchedulingCodeUpdateSuccessFault>();

            config.AddConsumer<SchedulingCodeUpdateFailedConsumer>();
            config.AddConsumer<SchedulingCodeUpdateFailedFault>();

            config.AddConsumer<SchedulingCodeDeleteSuccessConsumer>();
            config.AddConsumer<SchedulingCodeDeleteSuccessFault>();

            config.AddConsumer<SchedulingCodeDeleteFailedConsumer>();
            config.AddConsumer<SchedulingCodeDeleteFailedFault>();

            config.AddConsumer<AgentCategoryCreateSuccessConsumer>();
            config.AddConsumer<AgentCategoryCreateSuccessFault>();

            config.AddConsumer<AgentCategoryCreateFailedConsumer>();
            config.AddConsumer<AgentCategoryCreateFailedFault>();

            config.AddConsumer<AgentCategoryUpdateSuccessConsumer>();
            config.AddConsumer<AgentCategoryUpdateSuccessFault>();

            config.AddConsumer<AgentCategoryUpdateFailedConsumer>();
            config.AddConsumer<AgentCategoryUpdateFailedFault>();

            config.AddConsumer<AgentCategoryDeleteSuccessConsumer>();
            config.AddConsumer<AgentCategoryDeleteSuccessFault>();

            config.AddConsumer<AgentCategoryDeleteFailedConsumer>();
            config.AddConsumer<AgentCategoryDeleteFailedFault>();
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
            cfg.RegisterPublisher<CreateSchedulingCodeCommand>(MassTransitConstants.SchedulingCodeExchange);
            cfg.RegisterPublisher<UpdateSchedulingCodeCommand>(MassTransitConstants.SchedulingCodeExchange);
            cfg.RegisterPublisher<DeleteSchedulingCodeCommand>(MassTransitConstants.SchedulingCodeExchange);
            cfg.RegisterPublisher<CreateAgentCategoryCommand>(MassTransitConstants.AgentCategoryExchange);
            cfg.RegisterPublisher<UpdateAgentCategoryCommand>(MassTransitConstants.AgentCategoryExchange);
            cfg.RegisterPublisher<DeleteAgentCategoryCommand>(MassTransitConstants.AgentCategoryExchange);
        }

        /// <summary>
        ///  The extension method for registering the ClientEventQueue endpoint receiver
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="ctx"></param>
        private static void RegisterClientEventQueueReceiver(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
        {
            cfg.ReceiveEndpoint(MassTransitConstants.SchedulingCodeEventQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.SchedulingCodeExchange, MassTransitConstants.SchedulingCodeEventQueueBindingPattern);
                c.Consumer<SchedulingCodeCreateSuccessConsumer>(ctx);
                c.Consumer<SchedulingCodeCreateFailedConsumer>(ctx);

                c.Consumer<SchedulingCodeUpdateSuccessConsumer>(ctx);
                c.Consumer<SchedulingCodeUpdateFailedConsumer>(ctx);

                c.Consumer<SchedulingCodeDeleteSuccessConsumer>(ctx);
                c.Consumer<SchedulingCodeDeleteFailedConsumer>(ctx);
            });

            cfg.ReceiveEndpoint(MassTransitConstants.AgentCategoryEventQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.AgentCategoryExchange, MassTransitConstants.AgentCategoryEventQueueBindingPattern);
                c.Consumer<AgentCategoryCreateSuccessConsumer>(ctx);
                c.Consumer<AgentCategoryCreateFailedConsumer>(ctx);

                c.Consumer<AgentCategoryUpdateSuccessConsumer>(ctx);
                c.Consumer<AgentCategoryUpdateFailedConsumer>(ctx);

                c.Consumer<AgentCategoryDeleteSuccessConsumer>(ctx);
                c.Consumer<AgentCategoryDeleteFailedConsumer>(ctx);
            });
        }
    }
}