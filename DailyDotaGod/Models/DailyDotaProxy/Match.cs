using Newtonsoft.Json;
using System;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    internal class MatchMapping
    {
        public const string StartTime = "starttime_unix";
        public const string LiveStatus = "status";
        public const string Team1 = "team1";
        public const string Team2 = "team2";
        public const string BestOf = "series_type";
        public const string League = "league";
    }

    public enum LiveStatus
    {
        Upcoming = 0, 
        Live = 1
    }

    public class Match
    {
        [JsonProperty(MatchMapping.StartTime)]
        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTime StartTime { get; set; }

        [JsonProperty(MatchMapping.LiveStatus)]
        public LiveStatus LiveStatus { get; set; }

        [JsonProperty(MatchMapping.Team1)]
        public Team Team1 { get; set; }

        [JsonProperty(MatchMapping.Team2)]
        public Team Team2 { get; set; }

        [JsonProperty(MatchMapping.BestOf)]
        public int BestOf { get; set; }

        [JsonProperty(MatchMapping.League)]
        public League League { get; set; }
    }
}