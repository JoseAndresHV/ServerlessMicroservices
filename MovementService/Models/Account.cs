using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementService.Models
{
    public class Account
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
