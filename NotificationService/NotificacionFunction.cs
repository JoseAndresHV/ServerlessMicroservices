using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationService.Models;

namespace NotificationService
{
    public class NotificacionFunction
    {
        [FunctionName("Notification")]
        public async Task RunAsync(
            [ServiceBusTrigger("queue", Connection = "ServiceBusConnection")] string messageItem,
            [CosmosDB(Connection = "CosmosDBConnection")] CosmosClient client,
            ILogger log)
        {
            var message = JsonConvert.DeserializeObject<Message>(messageItem);
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                EmailTo = message.Email,
                MessageBody = $"Transaction alert: ${message.Amount} {message.Type} made to/from your account",
            };

            var container = client.GetDatabase("NotificationDb").GetContainer("Notifications");
            await container.CreateItemAsync(notification);
        }
    }
}
