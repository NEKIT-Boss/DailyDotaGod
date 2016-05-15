using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DailyDotaGod.Models
{
    class Team
    {
        [JsonProperty("team_name")]
        public string Name { get; set; }

        [JsonProperty("team_tag")]
        public string Tag { get; set; }

        bool? Occured { get; set; }

        public override string ToString()
        {
            return $"{Name} with tag: {Tag}";
        }
    }
}
