﻿using Css.Api.Core.EventBus.Commands.SchedulingCode;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.EventHandlers.Faults.SchedulingCode
{
    public class CreateSchedulingCodeCommandFault : IConsumer<Fault<CreateSchedulingCodeCommand>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<CreateSchedulingCodeCommand>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SchedulingCode Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
