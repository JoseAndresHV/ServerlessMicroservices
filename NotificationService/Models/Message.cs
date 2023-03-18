using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Models
{
    public class Message
    {
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public string Email { get; set; }
    }
}
