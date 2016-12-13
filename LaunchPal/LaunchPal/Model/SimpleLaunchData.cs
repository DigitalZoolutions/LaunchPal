using System;
using LaunchPal.Enums;
using LaunchPal.Helper;

namespace LaunchPal.Model
{
    public class SimpleLaunchData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
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
            this.Description = launchData.Mission.Description ?? launchData.Launch.Missions[0].Description ?? "No mission description";
            this.Status = LaunchStatusEnum.GetLaunchStatusStringById(LaunchStatusEnum.GetLaunchStatusById(launchData.Launch.Status), TimeConverter.DetermineTimeSettings(launchData.Launch.Net, App.Settings.UseLocalTime));
            this.Net = TimeConverter.DetermineTimeSettings(launchData.Launch.Net, App.Settings.UseLocalTime);
            this.LaunchNet = launchData.Launch.Status == 2 
                ? "TBD" 
                : TimeConverter.SetStringTimeFormat(launchData.Launch.Net, App.Settings.UseLocalTime);
        }
    }
}
