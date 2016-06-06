using DailyDotaGod.Data;
using DailyDotaGod.Models;
using DailyDotaGod.Models.DailyDotaProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DailyDotaGod.ViewModels
{
    class DailyDotaLoader: NotificationBase
    {
        private DailyDotaClient Client = DailyDotaClient.Instance;
        private StorageManager Storage = StorageManager.Instance;

        private DispatcherTimer ReloadTimer { get; set; } = null;

        private bool _isConnected = false;
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }

            set
            {
                SetProperty(ref _isConnected, value);
            }
        }

        private bool _connectionChecking = true;
        public bool ConnectionChecking
        {
            get
            {
                return _connectionChecking;
            }

            set
            {
                SetProperty(ref _connectionChecking, value);
            }
        }

        private bool _updated;
        public bool Updated
        {
            get
            {
                return _updated;
            }

            set
            {
                SetProperty(ref _updated, value);
            }
        }

        private TimeSpan _reloadInterval;
        public TimeSpan ReloadInterval
        {
            get
            {
                return _reloadInterval;
            }

            set
            {
                _reloadInterval = value;
                ReconfigureTimer();
            }
        }

        private async Task<bool> CheckConnectionAsync()
        {
            return await Task.Run(async () =>
            {
                const int TRIES_LIMIT = 5;
                TimeSpan retryDelay = TimeSpan.FromSeconds(0.5);

                for (int tryIndex = 0; tryIndex < TRIES_LIMIT; tryIndex++)
                {
                    if (Client.IsConnnected)
                    {
                        return true;
                    }
                    await Task.Delay(retryDelay).ConfigureAwait(false);
                }
                return false;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// For now it tries to load, and stuff
        /// </summary>
        /// <returns>Whether or not the client is connected after all</returns>
        private async Task<bool> LoadMatchesAsync()
        {
            return await Task.Run(async () =>
           {
               bool loadedAny = false;

               bool isConnected = await CheckConnectionAsync();
               Debug.WriteLine("checked");
               if (!isConnected)
               {
                   return loadedAny;
               }

               MatchesInfo matchesInfo = await Client.RequestMatchesInfoAsync();
               var teams = matchesInfo.Matches
                .SelectMany(x => new Models.DailyDotaProxy.Team[] { x.Team1, x.Team2 })
                .Distinct();

               var newTeams = from team in teams
                              where !Storage.TeamExists(team)
                              select team;

               try
               {
                   if (newTeams.Any())
                   {
                       Debug.WriteLine("StoringTeams");
                       await Storage.StoreTeams(newTeams);
                       Debug.WriteLine("TeamsStored");
                       loadedAny = true;
                   }
               }
               catch
               {
                   return false;
               }

               var newMatches = from match in matchesInfo.Matches
                             where !Storage.MatchExists(match)
                             select match;

               try
               {
                   if (newMatches.Any())
                   {
                       Debug.WriteLine("Started Matches");
                       await Storage.StoreMatches(newMatches);
                       loadedAny = true;
                   }
               }

               catch
               {
                   return false;
               }

               return loadedAny;
           }).ConfigureAwait(false);
        }

        private async void LoadMatchesAsyncEvent(object sender, object e)
        {
            (sender as DispatcherTimer).Stop();
            ConnectionChecking = true;
            IsConnected = await CheckConnectionAsync();

            if (IsConnected)
            {
                bool loadedAny = await LoadMatchesAsync();
                if (loadedAny)
                {
                    Debug.WriteLine("Loaded new!");
                }
            }

            ConnectionChecking = false;
            (sender as DispatcherTimer).Start();
        }

        private void ReconfigureTimer()
        {
            if (ReloadTimer.IsEnabled)
            {
                ReloadTimer.Stop();
                ReloadTimer.Interval = ReloadInterval;
                ReloadTimer.Start();
            }
        }

        private void ConfigureTimer()
        {
            ReloadTimer.Tick += LoadMatchesAsyncEvent;
            ReloadTimer.Interval = ReloadInterval;
        }

        public async Task StartRequesting()
        {
            ConnectionChecking = true;
            IsConnected = await CheckConnectionAsync();
            await LoadMatchesAsync();

            //ReloadTimer.Start();
            //ConnectionChecking = false;
        }

        public DailyDotaLoader(TimeSpan requestInterval)
        {
            ReloadTimer = new DispatcherTimer();
            ReloadInterval = requestInterval;
            ConfigureTimer();
        }
    }
}
