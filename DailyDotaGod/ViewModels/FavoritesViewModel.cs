using DailyDotaGod.Data;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.ViewModels
{
    class FavoritesViewModel : NotificationBase
    {
        private List<TeamViewModel> _teams;
        public List<TeamViewModel> Teams
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
            Teams = new List<TeamViewModel>();
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

                    Teams = await (from favorite in favorites
                            select new TeamViewModel(favorite.Team))
                            .ToAsyncEnumerable()
                            .ToList();

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
