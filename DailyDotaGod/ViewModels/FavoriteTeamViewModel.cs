using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyDotaGod.Data;
using DailyDotaGod.Models;

namespace DailyDotaGod.ViewModels
{
    class FavoriteTeamViewModel : TeamViewModel
    {
        private bool _deleteEnabled = false;
        public bool DeleteEnabled
        {
            get
            {
                return _deleteEnabled;
            }

            set
            {
                SetProperty(ref _deleteEnabled, value);
            }
        }

        public async Task<bool> DeleteAsync()
        {
            try
            {
                await StorageManager.Instance.RemoveFavoriteTeam(This);
                return true;
            }

            catch
            {
                return false;
            }

        }

        public FavoriteTeamViewModel(Team thing) : base(thing)
        {

        }


    }
}
