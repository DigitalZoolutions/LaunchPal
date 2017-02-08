using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LaunchPal.WinPhone.Models
{
    class NotificationData
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("launchId")]
        public string LaunchId { get; set; }
    }
}
