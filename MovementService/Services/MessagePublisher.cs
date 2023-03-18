using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementService.Services
{
    public class MessagePublisher
    {
        private readonly ServiceBusClient _busClient;

        public MessagePublisher(ServiceBusClient _busClient)
        {
            this._busClient = _busClient;
        }

        public async Task Publish<T>(string queueName, T data)
        {
            var sender = _busClient.CreateSender(queueName);

            var objAsText = JsonConvert.SerializeObject(data);
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(objAsText));

            try
            {
                await sender.SendMessageAsync(message);
            }
            finally
            {
                await sender.DisposeAsync();
            }
        }
    }
}
