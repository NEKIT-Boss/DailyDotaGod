using DailyDotaGod.Data;
using DailyDotaGod.Models;
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
    public sealed partial class FavoritesPage : Page
    {
        FavoritesViewModel FavoritesViewModel { get; set; }

        public FavoritesPage()
        {
            FavoritesViewModel = new FavoritesViewModel();
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame mainMenuFrame = Parent as Frame;
            mainMenuFrame.Navigate(typeof(AddFavoriteTeamPage), "Добавить команду");
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            await FavoritesViewModel.Load();
            AddTeamButton.Visibility = Visibility.Visible;
        }
    }
}
