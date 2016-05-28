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

        public FavoritesViewModel(IEnumerable<TeamViewModel> teams)
        {
            Teams = new List<TeamViewModel>(teams);
        }
    }
}
