using Newtonsoft.Json;
using System.Collections.Generic;

namespace DailyDotaGod.Models.SteamDotaAPIProxy
{
    internal static class MatchesDataMapping
    {
        public const string Matches = "games";
    }

    public class MatchesData
    {
        [JsonProperty(MatchesDataMapping.Matches)]
        public List<Match> Matches { get; set; }
    }
}