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

namespace AccountService
{
    public static class UpdateAccountFunction
    {
        [FunctionName("UpdateAccount")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "updateAccount")] Account updateAccount,
            [CosmosDB(Connection = "CosmosDBConnection")] CosmosClient client,
            ILogger log)
        {
            var container = client.GetDatabase("AccountDb").GetContainer("Accounts");
            var accountService = new Services.AccountService(container);

            var account = await accountService.Update(updateAccount);

            return new OkObjectResult(
                new Models.Response<Account>
                {
                    Success = account.Exists,
                    Data = account.Data,
                    Message = "Account updated"
                });
        }
    }
}
