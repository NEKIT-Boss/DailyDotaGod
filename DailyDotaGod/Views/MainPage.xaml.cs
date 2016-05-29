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
using DailyDotaGod.ViewModels;
using DailyDotaGod.Data;
using System.Threading.Tasks;
using System.Diagnostics;
using DailyDotaGod.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DailyDotaGod.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Shell AppShell { get; set; } = null;
        DailyDotaLoader Loader { get; set; } = null;

        public MainPage()
        {
            this.InitializeComponent();
            AppShell = new Shell();
            Loader = new DailyDotaLoader(TimeSpan.FromSeconds(25));
            MenuListBox.SelectedIndex = 0;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuListBox.SelectedIndex != -1)
            {
                ListBox menu = sender as ListBox;
                var pageNameTuple = AppShell.PageNameTuples[menu.SelectedIndex];

                MainMenuFrame.Navigate(pageNameTuple.Item1);
            }

        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            await Loader.StartRequesting();
        }

        private void MainMenuFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var pageNameTuple = AppShell.PageNameTuples.FirstOrDefault(x => x.Item1 == e.SourcePageType);
            if (pageNameTuple != null)
            {
                MenuListBox.SelectedIndex = AppShell.PageNameTuples.IndexOf(pageNameTuple);
                AppShell.MainMenuTitle = pageNameTuple.Item2;
            }

            else
            {
                MenuListBox.SelectedIndex = -1;
                AppShell.MainMenuTitle = e.Parameter as string;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuFrame.GoBack();
        }
    }
}
