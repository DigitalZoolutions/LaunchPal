using System;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.Model
{
    public class Notification
    {
        public Launch Launch { get; set; }
        public DateTime PushNotification { get; set; }
    }
}
