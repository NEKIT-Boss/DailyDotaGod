using DailyDotaGod.Data;
using DailyDotaGod.Models;
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
        private List<TeamViewModel> _teams = 
            new List<TeamViewModel>();
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

        public FavoritesViewModel()
        {
            Teams = new List<TeamViewModel>();
        }

        public void Filter()
        {
            Teams = StorageManager.Instance.Teams.Where(
                x => x.Name.ToLower()
                .Contains(SearchText.ToLower())
            ).ToList();
        }
    }
}
