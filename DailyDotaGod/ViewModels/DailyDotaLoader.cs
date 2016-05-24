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

        private async void RequestData(object sender = null, object e = null)
        {
            //Here we must request from the client, then,
            //feed that information to the KnowledgeManager
            //He will decide, whether to put or not the information into database again, and will tell all corresponding things
            //If they are to update
            ConnectionChecking = true;

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
                MatchesInfo matchesInfo = await Client.RequestMatchesInfoAsync();
                var teams = matchesInfo.Matches.Select(match => match.Team1).Union(matchesInfo.Matches.Select(match => match.Team2)).ToList();

                List<Team> newTeams = new List<Team>(); 
                foreach (Team team in teams)
                {
                    if (!Storage.TeamExists(team))
                    {
                        newTeams.Add(team);
                    }
                }

                if (newTeams.Count > 0)
                {
                    await Storage.StoreTeams(newTeams);
                }

                ConnectionChecking = false;
            }
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
                RequestData();
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
