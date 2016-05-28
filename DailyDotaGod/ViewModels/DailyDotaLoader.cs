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
               bool isConnected = await CheckConnectionAsync();
               if (!isConnected)
               {
                   return isConnected;
               }

               MatchesInfo matchesInfo = await Client.RequestMatchesInfoAsync();
               var teams = matchesInfo.Matches
                .SelectMany(x => new Team[] { x.Team1, x.Team2 })
                .Distinct();

               var newTeams = from team in teams
                              where !Storage.TeamExists(team)
                              select team;

               try
               {
                   if (newTeams.Any())
                   {
                       await Storage.StoreTeams(newTeams).ConfigureAwait(false);
                   }
               }
               catch
               {
                   return false;
               }

               return true;
           }).ConfigureAwait(false);
        }

        private async void LoadMatchesAsyncEvent(object sender, object e)
        {
            ConnectionChecking = true;
            IsConnected = await CheckConnectionAsync();

            if (IsConnected)
            {
                bool loadedAny = await LoadMatchesAsync();
                if (loadedAny)
                {
                    await Storage.SyncExposed(syncTeams: true);
                }
            }

            ConnectionChecking = false;
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
            bool loadedAny = await LoadMatchesAsync();
            if (loadedAny)
            {
                await Storage.SyncExposed();
            }

            ReloadTimer.Start();
            ConnectionChecking = false;
        }

        public DailyDotaLoader(TimeSpan requestInterval)
        {
            ReloadTimer = new DispatcherTimer();
            ReloadInterval = requestInterval;
            ConfigureTimer();
        }
    }
}
