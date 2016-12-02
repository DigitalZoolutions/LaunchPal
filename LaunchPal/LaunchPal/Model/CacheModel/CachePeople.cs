using System;
using System.Collections.Generic;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;

namespace LaunchPal.Model.CacheModel
{
    public class CachePeople : CacheBase
    {
        public List<Person> Astronouts { get; set; }

        public DateTime CacheTimeOut { get; set; }
    }
}
