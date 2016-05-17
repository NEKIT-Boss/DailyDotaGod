using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    internal static class MatchesInfoMapping
    {
        public const string ServerTime = "timestamp";
        public const string Matches = "matches";
    }

    public class MatchesInfo
    {
        [JsonProperty(MatchesInfoMapping.ServerTime)]
        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTime ServerTime { get; set; }

        [JsonProperty(MatchesInfoMapping.Matches)]
        public List<Match> Matches;
    }
}