using Microsoft.Azure.Cosmos;
using MovementService.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementService.Services
{
    public class TransactionService
    {

        private readonly Container _container;
        private readonly AccountService _accountService;

        public TransactionService(Container container, AccountService accountService)
        {
            _container = container;
            _accountService = accountService;
        }

        public async Task<Movement> Deposit(Account account, Transaction transaction)
        {
            account.TotalAmount += transaction.Amount;
            await _accountService.Update(account);

            var movement = new Movement
            {
                Id = Guid.NewGuid(),
                Amount = transaction.Amount,
                TotalAmount = account.TotalAmount,
                Type = "deposit",
                DateTime = DateTime.Now,
                AccountId = account.Id.ToString()
            };

            await _container.CreateItemAsync(movement);

            return movement;
        }
        public async Task<Movement> Withdrawal(Account account, Transaction transaction)
        {
            if (transaction.Amount > account.TotalAmount)
            {
                return null;
            }

            account.TotalAmount -= transaction.Amount;
            await _accountService.Update(account);

            var movement = new Movement
            {
                Id = Guid.NewGuid(),
                Amount = transaction.Amount,
                TotalAmount = account.TotalAmount,
                Type = "withdrawal",
                DateTime = DateTime.Now,
                AccountId = account.Id.ToString()
            };

            await _container.CreateItemAsync(movement);

            return movement;
        }
    }
}
