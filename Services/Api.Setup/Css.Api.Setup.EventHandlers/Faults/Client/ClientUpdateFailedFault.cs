﻿using Css.Api.Core.EventBus.Events.Client;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.Client
{

    public class ClientUpdateFailedFault : IConsumer<Fault<IClientUpdateFailed>>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<IClientUpdateFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. Client Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}

