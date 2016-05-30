using DailyDotaGod.Data;
using DailyDotaGod.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.ViewModels
{
    class FavoriteTeamAddingViewModel : NotificationBase
    {
        public List<TeamViewModel> Available { get; set; }

        private List<TeamViewModel> _shown = 
            new List<TeamViewModel>();
        public List<TeamViewModel> Shown
        {
            get
            {
                return _shown;
            }

            set
            {
                SetProperty(ref _shown, value);
            }
        }

        private string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                SetProperty(ref _searchText, value);
            }
        }

        public FavoriteTeamAddingViewModel()
        {
            
        }

        public async Task Load()
        {
            using (var context = new StorageContext())
            {
                context.TeamImages.Load();
                var teams = context.Teams.ToList()
                    .Except(context.FavoriteTeams.Select(x => x.Team))
                    .Select(x => new TeamViewModel(x));

                Available = teams.ToList();
            }
        }

        public async Task Filter()
        {
            Shown = await Available.Where(x => x.Name.ToLower().Contains(SearchText.ToLower())).ToAsyncEnumerable().ToList();            
        }
    }
}
