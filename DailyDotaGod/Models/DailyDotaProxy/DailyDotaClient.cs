using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    public sealed class DailyDotaClient
    {
        private HttpClient Client { get; set; }
        

        private static readonly Uri RequestAddress =
             new Uri(@"http://dailydota2.com/match-api");
        

        private static readonly DailyDotaClient _instance = new DailyDotaClient();
        public static DailyDotaClient Instance
        {
            get
            {
                return _instance;
            }
        } 


        public bool CheckConnectivity()
        {
            ConnectionProfile connection = NetworkInformation.GetInternetConnectionProfile();
            bool connected = (connection != null) && connection.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return connected;    
        }

        private DailyDotaClient()
        {
            Client = new HttpClient();
        }

        public async Task<MatchesInfo> RequestMatchesInfo()
        {
            return await Task.Run(() =>
            {
                string rawJson = Client.GetStringAsync(RequestAddress).Result.ToString();
                return JsonConvert.DeserializeObject<MatchesInfo>(rawJson);
            });
        }
    }
}
