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

namespace DailyDotaGod.ViewModels
{
    class AllMatchesViewModel : NotificationBase, IDisposable
    {
        CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

        private List<MatchViewModel> _matches = new List<MatchViewModel>();
        public List<MatchViewModel> Matches
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
            using (var context = new StorageContext())
            {
                await context.Teams.Include(x => x.Logo).LoadAsync();
                var matches = await context.Matches
                    .ToListAsync();

                Matches = await (from match in matches
                                 select new MatchViewModel(match))
                                .ToAsyncEnumerable()
                                .ToList();
            }
        }

        public void Dispose()
        {
            StorageManager.Instance.PropertyChanged -= StorageReloaded;
            Debug.WriteLine("Disposed"); 
        }
    }
}
