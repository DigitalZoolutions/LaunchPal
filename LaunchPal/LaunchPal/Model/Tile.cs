using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPal.Model
{
    public class Tile
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime Net { get; set; }
        public int LaunchId { get; set; }

        public Tile()
        {
            
        }

        public Tile(LaunchPair launchData)
        {
            this.LaunchId = launchData.Launch.Id;
            this.Name = launchData.Launch.Name;
            this.Message = launchData.Mission.Description;
            this.Net = launchData.Launch.Net;
        }
    }
}
