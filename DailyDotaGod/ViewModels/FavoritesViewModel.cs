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
        private ObservableCollection<TeamViewModel> _teams;
        public ObservableCollection<TeamViewModel> Teams
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
            Teams = new ObservableCollection<TeamViewModel>();
        }

        public void Delete(TeamViewModel team)
        {
            Teams.Remove(team);
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

                    Teams = new ObservableCollection<TeamViewModel>( await (from favorite in favorites
                            select new TeamViewModel(favorite.Team))
                            .ToAsyncEnumerable()
                            .ToList());

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
