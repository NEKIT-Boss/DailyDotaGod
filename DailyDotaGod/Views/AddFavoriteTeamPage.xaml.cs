using DailyDotaGod.Data;
using DailyDotaGod.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DailyDotaGod.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFavoriteTeamPage : Page
    {
        FavoriteTeamAddingViewModel AddingViewModel { get; set; } 

        public AddFavoriteTeamPage()
        {
            AddingViewModel = new FavoriteTeamAddingViewModel();
            this.InitializeComponent();
        }

        private async void SuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                using (var context = new StorageContext())
                {
                    Team chosen = (args.ChosenSuggestion as TeamViewModel);
                    context.FavoriteTeams.Add(new FavoriteTeam
                    {
                        Points = 100,
                        Team = chosen
                    });
                    await context.SaveChangesAsync();
                    (Parent as Frame).GoBack();
                }
            }
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            await AddingViewModel.Load();
        }
    }
}
