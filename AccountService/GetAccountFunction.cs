using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using AccountService.Models;

namespace AccountService
{
    public static class GetAccountFunction
    {
        [FunctionName("GetAccount")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getAccount/{id}")] HttpRequest req,
            string id,
            [CosmosDB(Connection = "CosmosDBConnection")] CosmosClient client,
            ILogger log)
        {
            var container = client.GetDatabase("AccountDb").GetContainer("Accounts");
            var accountService = new Services.AccountService(container);

            var account = await accountService.Get(id);

            return new OkObjectResult(
                new Models.Response<Account>
                {
                    Success = account.Exists,
                    Data = account.Data
                });
        }
    }
}
