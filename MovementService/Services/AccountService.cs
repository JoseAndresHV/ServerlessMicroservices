using Microsoft.Azure.Cosmos;
using MovementService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovementService.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _getEndpoint = "/getAccount/";
        private readonly string _updateEndpoint = "/updateAccount/";

        public AccountService(string apiBaseUrl)
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = apiBaseUrl;
        }

        public async Task<Account> Get(string accountId)
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl + _getEndpoint + accountId);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseAccount = JsonConvert.DeserializeObject<Models.Response<Account>>(responseContent);

                return responseAccount.Data;
            }
            catch 
            {
                return null;
            }
        }

        public async Task<bool> Update(Account account)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(_apiBaseUrl + _updateEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseAccount = JsonConvert.DeserializeObject<Models.Response<Account>>(responseContent);

                return responseAccount.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
