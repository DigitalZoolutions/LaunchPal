using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Model;

namespace LaunchPal.Interface
{
    public interface INotify
    {
        void AddNotification(Launch launch);

        void UpdateNotification(int index);

        void DeleteNotification(int index);

        void ClearNotifications();
    }
}
