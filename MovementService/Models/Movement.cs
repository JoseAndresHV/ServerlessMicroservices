using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementService.Models
{
    public class Movement
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public string AccountId { get; set; }
    }
}
