using Newtonsoft.Json;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    internal static class LeagueMapping
    {
        public const string Name = "name";
        public const string LogoUrl = "image_url";
    }

    public class League
    {
        [JsonProperty(LeagueMapping.Name)]
        public string Name { get; set; }

        [JsonProperty(LeagueMapping.LogoUrl)]
        private string LogoUrl { get; set; }

        [JsonIgnore]
        public BitmapImage LogoImage
        {
            get
            {
                return new BitmapImage(
                    new Uri(
                        LogoUrl,
                        UriKind.Absolute)
                    );
            }
        }
    }
}