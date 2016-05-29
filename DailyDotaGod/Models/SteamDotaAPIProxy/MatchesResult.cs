using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.Models.SteamDotaAPIProxy
{
    internal static class MatchesResultMapping
    {
        public const string Data = "result";
    }

    class MatchesResult
    {
        [JsonProperty(MatchesResultMapping.Data)]
        public MatchesData Data { get; set;}
    }
}
