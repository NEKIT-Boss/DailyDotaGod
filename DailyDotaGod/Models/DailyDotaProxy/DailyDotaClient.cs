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


        public bool IsConnnected
        {
            get
            {
                ConnectionProfile connection = NetworkInformation.GetInternetConnectionProfile();
                bool connected = (connection != null) && connection.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
                return connected;    
            }
        }

        private DailyDotaClient()
        {
        }

        public async Task<MatchesInfo> RequestMatchesInfoAsync()
        {
            return await Task.Run(async () =>
           {
               if (IsConnnected)
               {
                   using (var client = new HttpClient())
                   {
                       string rawJson = await client.GetStringAsync(RequestAddress).ConfigureAwait(false);
                       return await Task.Factory.StartNew(() =>
                       {
                           return JsonConvert.DeserializeObject<MatchesInfo>(rawJson);
                       }).ConfigureAwait(false);
                   }
               }

               else
               {
                   //Need to have some exception of some kind here
                   return null;
               }
           }).ConfigureAwait(false);
        }
    }
}
