using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Events.AgentSchedulingGroup;
using Css.Api.Core.EventBus.Events.Client;
using Css.Api.Core.EventBus.Events.ClientLOB;
using Css.Api.Core.EventBus.Events.SchedulingCode;
using Css.Api.Core.EventBus.Events.SkillGroup;
using Css.Api.Core.EventBus.Events.SkillTag;
using Css.Api.Scheduling.EventHandlers.Consumers.AgentSchedulingGroup;
using Css.Api.Scheduling.EventHandlers.Consumers.Client;
using Css.Api.Scheduling.EventHandlers.Consumers.ClientLOB;
using Css.Api.Scheduling.EventHandlers.Consumers.SchedulingCode;
using Css.Api.Scheduling.EventHandlers.Consumers.SkillGroup;
using Css.Api.Scheduling.EventHandlers.Consumers.SkillTag;
using Css.Api.Scheduling.EventHandlers.Faults.AgentSchedulingGroup;
using Css.Api.Scheduling.EventHandlers.Faults.Client;
using Css.Api.Scheduling.EventHandlers.Faults.ClientLOB;
using Css.Api.Scheduling.EventHandlers.Faults.SchedulingCode;
using Css.Api.Scheduling.EventHandlers.Faults.SkillGroup;
using Css.Api.Scheduling.EventHandlers.Faults.SkillTag;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;

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

            config.AddConsumer<UpdateClientCommandConsumer>();
            config.AddConsumer<UpdateClientCommandFault>();

            config.AddConsumer<DeleteClientCommandConsumer>();
            config.AddConsumer<DeleteClientCommandFault>();

            config.AddConsumer<CreateClientLOBCommandConsumer>();
            config.AddConsumer<CreateClientLOBCommandFault>();

            config.AddConsumer<UpdateClientLOBCommandConsumer>();
            config.AddConsumer<UpdateClientLOBCommandFault>();

            config.AddConsumer<DeleteClientLOBCommandConsumer>();
            config.AddConsumer<DeleteClientLOBCommandFault>();

            config.AddConsumer<CreateSkillGroupCommandConsumer>();
            config.AddConsumer<CreateSkillGroupCommandFault>();

            config.AddConsumer<UpdateSkillGroupCommandConsumer>();
            config.AddConsumer<UpdateSkillGroupCommandFault>();

            config.AddConsumer<DeleteSkillGroupCommandConsumer>();
            config.AddConsumer<DeleteSkillGroupCommandFault>();

            config.AddConsumer<CreateSkillTagCommandConsumer>();
            config.AddConsumer<CreateSkillTagCommandFault>();

            config.AddConsumer<UpdateSkillTagCommandConsumer>();
            config.AddConsumer<UpdateSkillTagCommandFault>();

            config.AddConsumer<DeleteSkillTagCommandConsumer>();
            config.AddConsumer<DeleteSkillTagCommandFault>();

            config.AddConsumer<CreateAgentSchedulingGroupCommandConsumer>();
            config.AddConsumer<CreateAgentSchedulingGroupCommandFault>();

            config.AddConsumer<UpdateAgentSchedulingGroupCommandConsumer>();
            config.AddConsumer<UpdateAgentSchedulingGroupCommandFault>();

            config.AddConsumer<DeleteAgentSchedulingGroupCommandConsumer>();
            config.AddConsumer<DeleteAgentSchedulingGroupCommandFault>();

            config.AddConsumer<CreateSchedulingCodeCommandConsumer>();
            config.AddConsumer<CreateSchedulingCodeCommandFault>();

            config.AddConsumer<UpdateSchedulingCodeCommandConsumer>();
            config.AddConsumer<UpdateSchedulingCodeCommandFault>();

            config.AddConsumer<DeleteSchedulingCodeCommandConsumer>();
            config.AddConsumer<DeleteSchedulingCodeCommandFault>();
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

            cfg.RegisterPublisher<IClientUpdateSuccess>(MassTransitConstants.ClientExchange);
            cfg.RegisterPublisher<IClientUpdateFailed>(MassTransitConstants.ClientExchange);

            cfg.RegisterPublisher<IClientDeleteSuccess>(MassTransitConstants.ClientExchange);
            cfg.RegisterPublisher<IClientDeleteFailed>(MassTransitConstants.ClientExchange);

            cfg.RegisterPublisher<IClientLOBCreateSuccess>(MassTransitConstants.ClientLOBExchange);
            cfg.RegisterPublisher<IClientLOBCreateFailed>(MassTransitConstants.ClientLOBExchange);

            cfg.RegisterPublisher<IClientLOBUpdateSuccess>(MassTransitConstants.ClientLOBExchange);
            cfg.RegisterPublisher<IClientLOBUpdateFailed>(MassTransitConstants.ClientLOBExchange);

            cfg.RegisterPublisher<IClientLOBDeleteSuccess>(MassTransitConstants.ClientLOBExchange);
            cfg.RegisterPublisher<IClientLOBDeleteFailed>(MassTransitConstants.ClientLOBExchange);

            cfg.RegisterPublisher<ISkillGroupCreateSuccess>(MassTransitConstants.SkillGroupExchange);
            cfg.RegisterPublisher<ISkillGroupCreateFailed>(MassTransitConstants.SkillGroupExchange);

            cfg.RegisterPublisher<ISkillGroupUpdateSuccess>(MassTransitConstants.SkillGroupExchange);
            cfg.RegisterPublisher<ISkillGroupUpdateFailed>(MassTransitConstants.SkillGroupExchange);

            cfg.RegisterPublisher<ISkillGroupDeleteSuccess>(MassTransitConstants.SkillGroupExchange);
            cfg.RegisterPublisher<ISkillGroupDeleteFailed>(MassTransitConstants.SkillGroupExchange);

            cfg.RegisterPublisher<ISkillTagCreateSuccess>(MassTransitConstants.SkillTagExchange);
            cfg.RegisterPublisher<ISkillTagCreateFailed>(MassTransitConstants.SkillTagExchange);

            cfg.RegisterPublisher<ISkillTagUpdateSuccess>(MassTransitConstants.SkillTagExchange);
            cfg.RegisterPublisher<ISkillTagUpdateFailed>(MassTransitConstants.SkillTagExchange);

            cfg.RegisterPublisher<ISkillTagDeleteSuccess>(MassTransitConstants.SkillTagExchange);
            cfg.RegisterPublisher<ISkillTagDeleteFailed>(MassTransitConstants.SkillTagExchange);

            cfg.RegisterPublisher<IAgentSchedulingGroupCreateSuccess>(MassTransitConstants.AgentSchedulingGroupExchange);
            cfg.RegisterPublisher<IAgentSchedulingGroupCreateFailed>(MassTransitConstants.AgentSchedulingGroupExchange);

            cfg.RegisterPublisher<IAgentSchedulingGroupUpdateSuccess>(MassTransitConstants.AgentSchedulingGroupExchange);
            cfg.RegisterPublisher<IAgentSchedulingGroupUpdateFailed>(MassTransitConstants.AgentSchedulingGroupExchange);

            cfg.RegisterPublisher<IAgentSchedulingGroupDeleteSuccess>(MassTransitConstants.AgentSchedulingGroupExchange);
            cfg.RegisterPublisher<IAgentSchedulingGroupDeleteFailed>(MassTransitConstants.AgentSchedulingGroupExchange);

            cfg.RegisterPublisher<ISchedulingCodeCreateSuccess>(MassTransitConstants.SchedulingCodeExchange);
            cfg.RegisterPublisher<ISchedulingCodeCreateFailed>(MassTransitConstants.SchedulingCodeExchange);

            cfg.RegisterPublisher<ISchedulingCodeUpdateSuccess>(MassTransitConstants.SchedulingCodeExchange);
            cfg.RegisterPublisher<ISchedulingCodeUpdateFailed>(MassTransitConstants.SchedulingCodeExchange);

            cfg.RegisterPublisher<ISchedulingCodeDeleteSuccess>(MassTransitConstants.SchedulingCodeExchange);
            cfg.RegisterPublisher<ISchedulingCodeDeleteFailed>(MassTransitConstants.SchedulingCodeExchange);
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
                c.Consumer<UpdateClientCommandConsumer>(ctx);
                c.Consumer<DeleteClientCommandConsumer>(ctx);
            });

            cfg.ReceiveEndpoint(MassTransitConstants.ClientLOBCommandQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.ClientLOBExchange, MassTransitConstants.ClientLOBCommandQueueBindingPattern);
                c.Consumer<CreateClientLOBCommandConsumer>(ctx);
                c.Consumer<UpdateClientLOBCommandConsumer>(ctx);
                c.Consumer<DeleteClientLOBCommandConsumer>(ctx);
            });


            cfg.ReceiveEndpoint(MassTransitConstants.SkillGroupCommandQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.SkillGroupExchange, MassTransitConstants.SkillGroupCommandQueueBindingPattern);
                c.Consumer<CreateSkillGroupCommandConsumer>(ctx);
                c.Consumer<UpdateSkillGroupCommandConsumer>(ctx);
                c.Consumer<DeleteSkillGroupCommandConsumer>(ctx);
            });

            cfg.ReceiveEndpoint(MassTransitConstants.SkillTagCommandQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.SkillTagExchange, MassTransitConstants.SkillTagCommandQueueBindingPattern);
                c.Consumer<CreateSkillTagCommandConsumer>(ctx);
                c.Consumer<UpdateSkillTagCommandConsumer>(ctx);
                c.Consumer<DeleteSkillTagCommandConsumer>(ctx);
            });

            cfg.ReceiveEndpoint(MassTransitConstants.AgentSchedulingGroupCommandQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.AgentSchedulingGroupExchange, MassTransitConstants.AgentSchedulingGroupCommandQueueBindingPattern);
                c.Consumer<CreateAgentSchedulingGroupCommandConsumer>(ctx);
                c.Consumer<UpdateAgentSchedulingGroupCommandConsumer>(ctx);
                c.Consumer<DeleteAgentSchedulingGroupCommandConsumer>(ctx);
            });

            cfg.ReceiveEndpoint(MassTransitConstants.SchedulingCodeCommandQueue, c =>
            {
                c.RegisterExchange(MassTransitConstants.SchedulingCodeExchange, MassTransitConstants.SchedulingCodeCommandQueueBindingPattern);
                c.Consumer<CreateSchedulingCodeCommandConsumer>(ctx);
                c.Consumer<UpdateSchedulingCodeCommandConsumer>(ctx);
                c.Consumer<DeleteSchedulingCodeCommandConsumer>(ctx);
            });
        }
    }
}