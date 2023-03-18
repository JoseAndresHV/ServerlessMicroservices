using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AccountService.Models;
using Microsoft.Azure.Cosmos;
using Azure.Messaging.ServiceBus;

namespace AccountService
{
    public static class CreateAccountFunction
    {
        [FunctionName("CreateAccount")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "createAccount")] CreateAccount newAcount,
            [CosmosDB(Connection = "CosmosDBConnection")] CosmosClient client,
            ILogger log)
        {
            var container = client.GetDatabase("AccountDb").GetContainer("Accounts");
            var accountService = new Services.AccountService(container);

            var account = await accountService.Create(newAcount);
            
            return new OkObjectResult(
                new Models.Response<Account>
                {
                    Success = true,
                    Data = account
                });
        }
    }
}
