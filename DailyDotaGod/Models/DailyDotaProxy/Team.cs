using Newtonsoft.Json;
using System;
using Windows.UI.Xaml.Media.Imaging;
using System.ServiceModel;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    internal static class TeamMapping
    {
        public const string Tag = "team_tag";
        public const string Name = "team_name";
        public const string LogoUrl = "logo_url";
        public const string CountryCode = "country_code";
    }

    public class Team
    {
        [JsonProperty(TeamMapping.Tag)]
        public string Tag { get; set; }

        [JsonProperty(TeamMapping.Name)]
        public string Name { get; set; }

        [JsonProperty(TeamMapping.LogoUrl)]
        private string LogoUrl { get; set; }

        [JsonIgnore]
        public BitmapImage LogoImage
        {
            get
            {
                const string imageUriPattern = @"http://dailydota2.com/{0}";

                return new BitmapImage(
                    new Uri(
                        string.Format(imageUriPattern, LogoUrl),
                        UriKind.Absolute)
                    );
            }
        }

        [JsonProperty(TeamMapping.CountryCode)]
        private string CountryCode { get; set; }

        [JsonIgnore]
        public BitmapImage CountryImage
        {
            get
            {
                const string imageUriPattern = @"http://dailydota2.com/images/cc/{0}.png";

                return new BitmapImage(
                    new Uri(
                        string.Format(imageUriPattern, CountryCode),
                        UriKind.Absolute)
                    );
            }
        }
    }
}