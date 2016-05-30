using DailyDotaGod.Data;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DailyDotaGod.ViewModels
{
    class SchedulerViewModel : NotificationBase
    {
        private TimeSpan SyncronizedInterval = TimeSpan.FromMinutes(1);
        private DispatcherTimer UpdateTimeTimer;


        private ObservableCollection<SchedulableMatchViewModel> _matches = new ObservableCollection<SchedulableMatchViewModel>();
        public ObservableCollection<SchedulableMatchViewModel> Matches
        {
            get
            {
                return _matches;
            }

            set
            {
                SetProperty(ref _matches, value);
            }
        }

        private void ConfigureTimer()
        {
            UpdateTimeTimer = new DispatcherTimer();
            UpdateTimeTimer.Tick += UpdateTimeTimer_Tick;
            int secondsTillMinute = DateTime.Now.Second;
            UpdateTimeTimer.Interval = secondsTillMinute > 10 ? TimeSpan.FromSeconds(secondsTillMinute) : TimeSpan.FromSeconds(10);
        }

        private void UpdateTimeTimer_Tick(object sender, object e)
        {
            foreach (var match in Matches)
            {
                match.TimeLeft = match.TimeLeft.Subtract(TimeSpan.FromMinutes(1));
            }

            if (UpdateTimeTimer.Interval != SyncronizedInterval)
            {
                UpdateTimeTimer.Stop();
                UpdateTimeTimer.Interval = SyncronizedInterval;
                UpdateTimeTimer.Start();
            }
        }

        public SchedulerViewModel()
        {
            ConfigureTimer();        
        }

        public async Task Load()
        {
            using (var context = new StorageContext())
            {
                await context.Teams
                    .Include(x => x.Logo)
                    .Include(x => x.CountryLogo)
                    .LoadAsync();

                var scheduled = await context.ScheduledMatches.Select(x => x.Match).ToListAsync();
                foreach (var match in scheduled)
                {
                    Matches.Add(new SchedulableMatchViewModel(match));
                }

                UpdateTimeTimer.Start();
            }
        }
    }
}
