using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.Model
{
    public class Notification
    {
        public Launch Launch { get; set; }
        public DateTime PushNotification { get; set; }
    }
}
