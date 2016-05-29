using DailyDotaGod.Data;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.ViewModels
{
    class SchedulerViewModel : NotificationBase
    {
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

        public SchedulerViewModel()
        {
                 
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
            }
        }
    }
}
