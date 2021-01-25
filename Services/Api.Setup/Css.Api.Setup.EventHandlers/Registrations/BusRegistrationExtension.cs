using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentSchedulingGroup;
using Css.Api.Core.EventBus.Commands.Client;
using Css.Api.Core.EventBus.Commands.ClientLOB;
using Css.Api.Core.EventBus.Commands.SkillGroup;
using Css.Api.Core.EventBus.Commands.SkillTag;
using Css.Api.Setup.EventHandlers.Consumers.AgentSchedulingGroup;
using Css.Api.Setup.EventHandlers.Consumers.Client;
using Css.Api.Setup.EventHandlers.Consumers.ClientLOB;
using Css.Api.Setup.EventHandlers.Consumers.SkillGroup;
using Css.Api.Setup.EventHandlers.Consumers.SkillTag;
using Css.Api.Setup.EventHandlers.Faults.AgentSchedulingGroup;
using Css.Api.Setup.EventHandlers.Faults.Client;
using Css.Api.Setup.EventHandlers.Faults.ClientLOB;
using Css.Api.Setup.EventHandlers.Faults.SkillGroup;
using Css.Api.Setup.EventHandlers.Faults.SkillTag;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;

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

            config.AddConsumer<ClientUpdateSuccessConsumer>();
            config.AddConsumer<ClientUpdateSuccessFault>();

            config.AddConsumer<ClientUpdateFailedConsumer>();
            config.AddConsumer<ClientUpdateFailedFault>();

            config.AddConsumer<ClientDeleteSuccessConsumer>();
            config.AddConsumer<ClientDeleteSuccessFault>();

            config.AddConsumer<ClientDeleteFailedConsumer>();
            config.AddConsumer<ClientDeleteFailedFault>();

            config.AddConsumer<ClientLOBCreateSuccessConsumer>();
            config.AddConsumer<ClientLOBCreateSuccessFault>();

            config.AddConsumer<ClientLOBCreateFailedConsumer>();
            config.AddConsumer<ClientLOBCreateFailedFault>();

            config.AddConsumer<ClientLOBUpdateSuccessConsumer>();
            config.AddConsumer<ClientLOBUpdateSuccessFault>();

            config.AddConsumer<ClientLOBUpdateFailedConsumer>();
            config.AddConsumer<ClientLOBUpdateFailedFault>();

            config.AddConsumer<ClientLOBDeleteSuccessConsumer>();
            config.AddConsumer<ClientLOBDeleteSuccessFault>();

            config.AddConsumer<ClientLOBDeleteFailedConsumer>();
            config.AddConsumer<ClientLOBDeleteFailedFault>();

            config.AddConsumer<SkillGroupCreateSuccessConsumer>();
            config.AddConsumer<SkillGroupCreateSuccessFault>();

            config.AddConsumer<SkillGroupCreateFailedConsumer>();
            config.AddConsumer<SkillGroupCreateFailedFault>();

            config.AddConsumer<SkillGroupUpdateSuccessConsumer>();
            config.AddConsumer<SkillGroupUpdateSuccessFault>();

            config.AddConsumer<SkillGroupUpdateFailedConsumer>();
            config.AddConsumer<SkillGroupUpdateFailedFault>();

            config.AddConsumer<SkillGroupDeleteSuccessConsumer>();
            config.AddConsumer<SkillGroupDeleteSuccessFault>();

            config.AddConsumer<SkillGroupDeleteFailedConsumer>();
            config.AddConsumer<SkillGroupDeleteFailedFault>();

            config.AddConsumer<SkillTagCreateSuccessConsumer>();
            config.AddConsumer<SkillTagCreateSuccessFault>();

            config.AddConsumer<SkillTagCreateFailedConsumer>();
            config.AddConsumer<SkillTagCreateFailedFault>();

            config.AddConsumer<SkillTagUpdateSuccessConsumer>();
            config.AddConsumer<SkillTagUpdateSuccessFault>();

            config.AddConsumer<SkillTagUpdateFailedConsumer>();
            config.AddConsumer<SkillTagUpdateFailedFault>();

            config.AddConsumer<SkillTagDeleteSuccessConsumer>();
            config.AddConsumer<SkillTagDeleteSuccessFault>();

            config.AddConsumer<SkillTagDeleteFailedConsumer>();
            config.AddConsumer<SkillTagDeleteFailedFault>();

            config.AddConsumer<AgentSchedulingGroupCreateSuccessConsumer>();
            config.AddConsumer<AgentSchedulingGroupCreateSuccessFault>();

            config.AddConsumer<AgentSchedulingGroupCreateFailedConsumer>();
            config.AddConsumer<AgentSchedulingGroupCreateFailedFault>();

            config.AddConsumer<AgentSchedulingGroupUpdateSuccessConsumer>();
            config.AddConsumer<AgentSchedulingGroupUpdateSuccessFault>();

            config.AddConsumer<AgentSchedulingGroupUpdateFailedConsumer>();
            config.AddConsumer<AgentSchedulingGroupUpdateFailedFault>();

            config.AddConsumer<AgentSchedulingGroupDeleteSuccessConsumer>();
            config.AddConsumer<AgentSchedulingGroupDeleteSuccessFault>();

            config.AddConsumer<AgentSchedulingGroupDeleteFailedConsumer>();
            config.AddConsumer<AgentSchedulingGroupDeleteFailedFault>();

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
            cfg.RegisterPublisher<UpdateClientCommand>(MassTransitConstants.ClientExchange);
            cfg.RegisterPublisher<DeleteClientCommand>(MassTransitConstants.ClientExchange);
            cfg.RegisterPublisher<CreateClientLOBCommand>(MassTransitConstants.ClientLOBExchange);
            cfg.RegisterPublisher<UpdateClientLOBCommand>(MassTransitConstants.ClientLOBExchange);
            cfg.RegisterPublisher<DeleteClientLOBCommand>(MassTransitConstants.ClientLOBExchange);
            cfg.RegisterPublisher<CreateSkillGroupCommand>(MassTransitConstants.SkillGroupExchange);
            cfg.RegisterPublisher<UpdateSkillGroupCommand>(MassTransitConstants.SkillGroupExchange);
            cfg.RegisterPublisher<DeleteSkillGroupCommand>(MassTransitConstants.SkillGroupExchange);
            cfg.RegisterPublisher<CreateSkillTagCommand>(MassTransitConstants.SkillTagExchange);
            cfg.RegisterPublisher<UpdateSkillTagCommand>(MassTransitConstants.SkillTagExchange);
            cfg.RegisterPublisher<DeleteSkillTagCommand>(MassTransitConstants.SkillTagExchange);
            cfg.RegisterPublisher<CreateAgentSchedulingGroupCommand>(MassTransitConstants.AgentSchedulingGroupExchange);
            cfg.RegisterPublisher<UpdateAgentSchedulingGroupCommand>(MassTransitConstants.AgentSchedulingGroupExchange);
            cfg.RegisterPublisher<DeleteAgentSchedulingGroupCommand>(MassTransitConstants.AgentSchedulingGroupExchange);

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

                c.Consumer<ClientUpdateSuccessConsumer>(ctx);
                c.Consumer<ClientUpdateFailedConsumer>(ctx);

                c.Consumer<ClientDeleteSuccessConsumer>(ctx);
                c.Consumer<ClientDeleteFailedConsumer>(ctx);
            });

            cfg.ReceiveEndpoint(MassTransitConstants.ClientLOBEventQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.ClientLOBExchange, MassTransitConstants.ClientLOBEventQueueBindingPattern);
                c.Consumer<ClientLOBCreateSuccessConsumer>(ctx);
                c.Consumer<ClientLOBCreateFailedConsumer>(ctx);

                c.Consumer<ClientLOBUpdateSuccessConsumer>(ctx);
                c.Consumer<ClientLOBUpdateFailedConsumer>(ctx);

                c.Consumer<ClientLOBDeleteSuccessConsumer>(ctx);
                c.Consumer<ClientLOBDeleteFailedConsumer>(ctx);
            });

            cfg.ReceiveEndpoint(MassTransitConstants.SkillGroupEventQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.SkillGroupExchange, MassTransitConstants.SkillGroupEventQueueBindingPattern);
                c.Consumer<SkillGroupCreateSuccessConsumer>(ctx);
                c.Consumer<SkillGroupCreateFailedConsumer>(ctx);

                c.Consumer<SkillGroupUpdateSuccessConsumer>(ctx);
                c.Consumer<SkillGroupUpdateFailedConsumer>(ctx);

                c.Consumer<SkillGroupDeleteSuccessConsumer>(ctx);
                c.Consumer<SkillGroupDeleteFailedConsumer>(ctx);
            });


            cfg.ReceiveEndpoint(MassTransitConstants.AgentSchedulingGroupEventQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.AgentSchedulingGroupExchange, MassTransitConstants.AgentSchedulingGroupEventQueueBindingPattern);
                c.Consumer<AgentSchedulingGroupCreateSuccessConsumer>(ctx);
                c.Consumer<AgentSchedulingGroupCreateFailedConsumer>(ctx);

                c.Consumer<AgentSchedulingGroupUpdateSuccessConsumer>(ctx);
                c.Consumer<AgentSchedulingGroupUpdateFailedConsumer>(ctx);

                c.Consumer<AgentSchedulingGroupDeleteSuccessConsumer>(ctx);
                c.Consumer<AgentSchedulingGroupDeleteFailedConsumer>(ctx);
            });


        }
    }
}
