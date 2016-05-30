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
    class FavoritesViewModel : NotificationBase
    {
        private ObservableCollection<FavoriteTeamViewModel> _teams = new ObservableCollection<FavoriteTeamViewModel>();
        public ObservableCollection<FavoriteTeamViewModel> Teams
        {
            get
            {
                return _teams;
            }

            set
            {
                SetProperty(ref _teams, value);
            }
        }

        public FavoritesViewModel()
        {
        }

        public async Task<bool> Load()
        {
            using (var context = new StorageContext())
            {
                try
                {
                    var favorites = await context.FavoriteTeams
                        .Include( x => x.Team )
                        .ThenInclude( x => x.Logo )
                        .ToListAsync();

                    foreach (var favorite in favorites)
                    {
                        Teams.Add(new FavoriteTeamViewModel(favorite.Team));
                    }
                    
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
