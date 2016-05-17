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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DailyDotaGod.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Shell AppShell { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            AppShell = new Shell(null, MainMenuFrame);
            MenuListBox.SelectedIndex = 0;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox menu = sender as ListBox;

            var selectedItem = menu.SelectedItem as ListBoxItem;
            AppShell.NavigateMenu(selectedItem.Tag.ToString());
        }
    }
}
