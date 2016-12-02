using System;
using LaunchPal.Helper;

namespace LaunchPal.Model
{
    public class SimpleLaunchData
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime Net { get; set; }
        public string LaunchNet { get; set; }
        public int LaunchId { get; set; }

        public SimpleLaunchData()
        {
            
        }

        public SimpleLaunchData(LaunchData launchData)
        {
            this.LaunchId = launchData.Launch.Id;
            this.Name = launchData.Launch.Name;
            this.Message = launchData.Mission.Description ?? launchData.Launch.Missions[0].Description ?? "No mission description";
            this.Net = TimeConverter.DetermineTimeSettings(launchData.Launch.Net, App.Settings.UseLocalTime);
            this.LaunchNet = launchData.Launch.Status == 2 
                ? "TBD" 
                : TimeConverter.DetermineTimeSettings(launchData.Launch.Net, App.Settings.UseLocalTime).ToString();
        }
    }
}
