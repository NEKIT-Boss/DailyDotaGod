using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableSteam;
using DailyDotaGod.Data;
using Microsoft.Data.Entity;
using System.Diagnostics;
using DailyDotaGod.Models;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Collections.ObjectModel;

namespace DailyDotaGod.ViewModels
{
    class AllMatchesViewModel : NotificationBase, IDisposable
    {
        CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        private bool _isLoaded = false;
        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }

            set
            {
                SetProperty(ref _isLoaded, value);
            }
        }

        private ObservableCollection<SchedulableMatchViewModel> _upcomingLiveMatches = new ObservableCollection<SchedulableMatchViewModel>();
        public ObservableCollection<SchedulableMatchViewModel> UpcomingLiveMatches
        {
            get
            {
                return _upcomingLiveMatches;
            }

            set
            {
                SetProperty(ref _upcomingLiveMatches, value);
            }
        }

        private ObservableCollection<MatchViewModel> _recentMatches = new ObservableCollection<MatchViewModel>();
        public ObservableCollection<MatchViewModel> RecentMatches
        {
            get
            {
                return _recentMatches;
            }

            set
            {
                SetProperty(ref _recentMatches, value);
            }
        }

        public AllMatchesViewModel()
        {
            StorageManager.Instance.PropertyChanged += StorageReloaded;
        }

        private async void StorageReloaded(object sender, PropertyChangedEventArgs e)
        {
            await dispatcher.RunAsync( CoreDispatcherPriority.Normal, async () => await Load());
        }

        public async Task Load()
        {
            IsLoaded = false;

            RecentMatches.Clear();
            UpcomingLiveMatches.Clear();

            using (var context = new StorageContext())
            {
                await context.Teams.Include(x => x.Logo).LoadAsync();
                var matches = await context.Matches
                    .ToListAsync();

                foreach (var match in matches)
                {
                    if (match.Expired())
                    {
                        RecentMatches.Add(new MatchViewModel(match));
                    }
                    else
                    {
                        UpcomingLiveMatches.Add(new SchedulableMatchViewModel(match));
                    }
                }
            }
            IsLoaded = true;
        }

        public void Dispose()
        {
            StorageManager.Instance.PropertyChanged -= StorageReloaded;
            Debug.WriteLine("Disposed"); 
        }
    }
}
