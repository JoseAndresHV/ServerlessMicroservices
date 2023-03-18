using AccountService.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Services
{
    public class AccountService
    {
        private readonly Container _container;

        public AccountService(Container container)
        {
            _container = container;
        }

        public async Task<Account> Create(CreateAccount newAccount)
        {
            var account = new Account {
                Id = Guid.NewGuid(),
                FullName = newAccount.FullName,
                Email = newAccount.Email,   
                TotalAmount = newAccount.TotalAmount
            };

            await _container.CreateItemAsync(account);

            return account;
        }

        public async Task<(Account Data, bool Exists)> Get(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Account>(id, new PartitionKey(id));
                return (response.Resource, true);
            }
            catch
            {
                return (null, false);
            }
        }

        public async Task<(Account Data, bool Exists)> Update(Account account)
        {
            try
            {
                var id = account.Id.ToString();
                var response = await _container.ReplaceItemAsync(account, id, new PartitionKey(id));
                return (response.Resource, true);
            }
            catch
            {
                return (null, false);
            }
        }
    }
}
