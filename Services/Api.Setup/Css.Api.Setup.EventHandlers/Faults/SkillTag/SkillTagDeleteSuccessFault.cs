﻿using Css.Api.Core.EventBus.Events.SkillTag;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Css.Api.Setup.EventHandlers.Faults.SkillTag
{
    public class SkillTagDeleteSuccessFault : IConsumer<Fault<ISkillTagDeleteSuccess>>
    {
        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task Consume(ConsumeContext<Fault<ISkillTagDeleteSuccess>> context)
        {
            Console.WriteLine($"request with message id {context.Message.FaultedMessageId} failed. SkillTag Id: {context.Message.Message.Id}");
            return Task.CompletedTask;
        }
    }
}