using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Models
{
    public class Notification
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string EmailTo { get; set; }
        public string MessageBody { get; set; }
    }
}
