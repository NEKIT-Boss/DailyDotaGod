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
using DailyDotaGod.Models;
using Newtonsoft.Json;
using DailyDotaGod.Models.DailyDotaProxy;
using DailyDotaGod.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DailyDotaGod.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            
            this.InitializeComponent();
        }

        public string OneManArmy { get; set; }

        private async void LoadingPage(FrameworkElement sender, object args)
        {
            MatchesInfo matches = await DailyDotaClient.Instance.RequestMatchesInfo();
            using (var context = new StorageContext())
            {
                Woody.Text = context.Teams.First().Name;
            }
        }
    }
}
