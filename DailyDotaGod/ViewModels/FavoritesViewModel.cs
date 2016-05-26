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
        private ObservableCollection<TeamViewModel> _teams = 
            new ObservableCollection<TeamViewModel>();
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

        public FavoritesViewModel(ObservableCollection<TeamViewModel> teams)
        {
            Teams = teams;
        }
    }
}
