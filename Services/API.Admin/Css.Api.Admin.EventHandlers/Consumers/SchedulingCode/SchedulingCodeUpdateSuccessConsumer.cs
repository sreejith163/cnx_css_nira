﻿using Css.Api.Core.EventBus.Events.SchedulingCode;
using MassTransit;
using System.Threading.Tasks;

namespace Css.Api.Admin.EventHandlers.Consumers.SchedulingCode
{
    public class SchedulingCodeUpdateSuccessConsumer : IConsumer<ISchedulingCodeUpdateSuccess>
    {

        /// <summary>Consumes the specified context.</summary>
        /// <param name="context">The context.</param>
        public async Task Consume(ConsumeContext<ISchedulingCodeUpdateSuccess> context)
        {
            var message = context.Message;
        }
    }
}
