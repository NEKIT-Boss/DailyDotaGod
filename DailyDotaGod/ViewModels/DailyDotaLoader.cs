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

        private DispatcherTimer RequestTimer { get; set; } = null;

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

        private bool _connectionChecking = false;
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

        private TimeSpan _requestInterval;
        public TimeSpan RequestInterval
        {
            get
            {
                return _requestInterval;
            }

            set
            {
                _requestInterval = value;
                ReconfigureTimer();
            }
        }

        private async void RequestData(object sender, object e)
        {
            (sender as DispatcherTimer).Stop();

            ConnectionChecking = true;

            // Need to put that in config file
            const int RECHECK_COUNT = 5;
            for (int recheck = 0; recheck < RECHECK_COUNT; recheck++)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(800));
                if (IsConnected = Client.IsConnnected)
                {
                    break;
                }
            }

            if (IsConnected)
            {
                //Debug.WriteLine("Stepped into async load");
                MatchesInfo matchesInfo = await Client.RequestMatchesInfoAsync();
                //Debug.WriteLine("Stepped out async load");

                var teams = matchesInfo.Matches.Select(match => match.Team1).Union(matchesInfo.Matches.Select(match => match.Team2)).ToList();

                List<Team> newTeams = new List<Team>(); 
                foreach (Team team in teams)
                {
                    if (! await Storage.TeamExists(team) )
                    {
                        newTeams.Add(team);
                    }
                }

                if (newTeams.Count > 0)
                {
                    await Storage.StoreTeams(newTeams);
                }
            }

            await Storage.SyncExposed();
            ConnectionChecking = false;
        }

        private void ReconfigureTimer()
        {
            if (RequestTimer.IsEnabled)
            {
                RequestTimer.Stop();
                RequestTimer.Interval = RequestInterval;
                RequestTimer.Start();
            }
        }

        private void ConfigureTimer()
        {
            RequestTimer.Tick += RequestData;
            RequestTimer.Interval = RequestInterval;
        }

        public void StartRequesting()
        {
            IsConnected = Client.IsConnnected;

            if (IsConnected)
            {
                //Need to think about tricking timer, and consider real Task thing
                //RequestData();
                RequestTimer.Start();
            }
            else
            {
                ConnectionChecking = false;
                RequestTimer.Start();
            }
        }

        public DailyDotaLoader(TimeSpan requestInterval)
        {
            RequestTimer = new DispatcherTimer();
            RequestInterval = requestInterval;
            ConfigureTimer();
        }
    }
}
