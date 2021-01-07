﻿using Css.Api.Core.EventBus.Events.ClientLOB;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.ClientLOB
{

    public class ClientLOBDeleteFailedFault : IConsumer<Fault<IClientLOBDeleteFailed>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<IClientLOBDeleteFailed>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. ClientLOB Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}