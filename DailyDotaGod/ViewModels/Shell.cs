using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Controls;
using DailyDotaGod.Views;
using PageDictionary = System.Collections.Generic.Dictionary<string, System.Type>;
using Windows.UI.Xaml;

namespace DailyDotaGod.ViewModels
{
    class Shell : NotificationBase
    {
        public List<Tuple<Type, string>> PageNameTuples { get; set; }

        private string _mainMenuTitle;
        public string MainMenuTitle
        {
            get
            {
                return _mainMenuTitle;
            }

            set
            {
                SetProperty(ref _mainMenuTitle, value);
            }
        }

        public Shell()
        {
            PageNameTuples = new List<Tuple<Type, string>>
            {
                Tuple.Create(typeof(HomePage), "Сводка"),
                Tuple.Create(typeof(AllMatchesPage), "Все Матчи"),
                Tuple.Create(typeof(HomePage), "В эфире"),
                Tuple.Create(typeof(HomePage), "Мое Расписание"),
                Tuple.Create(typeof(FavoritesPage), "Избранное")
            };
        }
    }
}
