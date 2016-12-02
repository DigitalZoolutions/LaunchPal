using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.PeopleInSpace.JsonObject
{
    public class PeopleInSpace : PeopleBase
    {
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("people")]
        public List<Person> People { get; set; }
    }

    public class Person
    {
        private string _launchdate;

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("biophoto")]
        public string Biophoto { get; set; }
        [JsonProperty("biophotowidth")]
        public int Biophotowidth { get; set; }
        [JsonProperty("biophotoheight")]
        public int Biophotoheight { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("countryflag")]
        public string Countryflag { get; set; }

        [JsonProperty("launchdate")]
        public string Launchdate
        {
            get { return _launchdate; }
            set
            {
                _launchdate = value;
                DaysInSpace = (DateTime.Now - DateTime.Parse(_launchdate)).Days;
            }
        }

        [JsonProperty("careerdays")]
        public int Careerdays { get; set; }

        public int DaysInSpace { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("bio")]
        public string Bio { get; set; }
        [JsonProperty("biolink")]
        public string Biolink { get; set; }
        [JsonProperty("twitter")]
        public string Twitter { get; set; }
    }
}
