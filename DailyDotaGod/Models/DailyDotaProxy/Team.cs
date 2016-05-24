using Newtonsoft.Json;
using System;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using System.ServiceModel;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Storage.Streams;
using System.Collections.Generic;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    internal static class TeamMapping
    {
        public const string Tag = "team_tag";
        public const string Name = "team_name";
        public const string LogoEndpoint = "logo_url";
        public const string CountryCode = "country_code";
    }

    public class Team : IEquatable<Team>
    {
        [JsonProperty(TeamMapping.Tag)]
        public string Tag { get; set; }

        [JsonProperty(TeamMapping.Name)]
        public string Name { get; set; }

        [JsonProperty(TeamMapping.LogoEndpoint)]
        private string LogoEndpoint { get; set; }

        [JsonIgnore]
        public Uri LogoUrl
        {
            get
            {
                const string imageUriPattern = @"http://dailydota2.com{0}";
                return new Uri(string.Format(imageUriPattern, LogoEndpoint), UriKind.Absolute);
            }
        }

        [JsonProperty(TeamMapping.CountryCode)]
        public string CountryCode { get; set; }

        [JsonIgnore]
        public Uri CountryLogoUrl
        {
            get
            {
                const string imageUriPattern = @"http://dailydota2.com/images/cc/{0}.png";
                return new Uri( string.Format(imageUriPattern, CountryCode), UriKind.Absolute);
            }
        }

        public bool Equals(Team other)
        {
            return (Name == other.Name) && (Tag == other.Tag);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Tag.GetHashCode();
        }
    }
}