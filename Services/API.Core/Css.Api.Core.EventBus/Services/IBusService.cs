using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Core.EventBus.Services
{
    public interface IBusService
    {
        Task SendCommand<T>(string queueName, object values)
           where T : class;
        Task PublishEvent<T>(string routingKey, object values)
            where T : class;
    }
}
