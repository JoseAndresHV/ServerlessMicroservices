using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MovementService.Models;
using MovementService.Services;
using Microsoft.Azure.Cosmos;
using System.Configuration;
using Azure.Messaging.ServiceBus;

namespace MovementService
{
    public static class DepositFunction
    {
        [FunctionName("Deposit")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "deposit")] Transaction transaction,
            [CosmosDB(Connection = "CosmosDBConnection")] CosmosClient client,
            ILogger log)
        {
            var accountService = new AccountService(Environment.GetEnvironmentVariable("AccountServiceApiUrl"));
            var account = await accountService.Get(transaction.AccountId);

            if (account == null)
            {
                return new OkObjectResult(
                    new Models.Response<Account>
                    {
                        Success = false,
                        Data = null,
                        Message = "Account not found"
                    });
            }

            var container = client.GetDatabase("MovementDb").GetContainer("Movements");
            var transactionService = new TransactionService(container, accountService);
            var movement = await transactionService.Deposit(account, transaction);

            var serviceBusClient = new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnection"));
            var publisher = new MessagePublisher(serviceBusClient);
            await publisher.Publish("queue", new Message
            {
                Amount = movement.Amount,
                Type = movement.Type,
                DateTime = movement.DateTime,
                Email = account.Email,
            });

            return new OkObjectResult(
                    new Models.Response<Movement>
                    {
                        Success = true,
                        Data = movement,
                        Message = "Deposit registered"
                    });
        }
    }
}
