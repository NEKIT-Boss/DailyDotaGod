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
        private PageDictionary MainFramePages { get; set; }
        private Frame MainFrame { get; set; }

        private Frame MainMenuFrame { get; set; }
        private PageDictionary MainMenuPages { get; set; }

        private DispatcherTimer NetworkCheckTimer = new DispatcherTimer();

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

        private bool _isNetworkChecked = false;
        public bool IsNetworkChecked
        {
            get
            {
                return _isNetworkChecked;
            }

            set
            {
                SetProperty(ref _isNetworkChecked, value);
            }
        }

        public Shell(Frame mainFrame, Frame mainMenuFrame)
        {
            MainFrame = mainFrame;
            MainMenuFrame = mainMenuFrame;

            MainMenuPages = new PageDictionary()
            {
                { "Сводка", typeof(HomePage) },
                { "Все Матчи", typeof(AllMatchesPage) }
            };

            NavigateMenu("Сводка");
            NetworkCheckTimer.Tick += CheckNetwork;
            NetworkCheckTimer.Interval = TimeSpan.FromSeconds(10);

            CheckNetwork(null, null);
            NetworkCheckTimer.Start();
        }

        private async void CheckNetwork(object sender, object e)
        {
            IsNetworkChecked = false;
            await Task.Delay(TimeSpan.FromSeconds(4));
            IsNetworkChecked = true;
        }
        
        public void NavigateMenu(string pageName)
        {
            Type nextPage = null;
            if (MainMenuPages.TryGetValue(pageName, out nextPage))
            {
                MainMenuTitle = pageName;
                MainMenuFrame.Navigate(nextPage);
            }
        }



       
    }
}
