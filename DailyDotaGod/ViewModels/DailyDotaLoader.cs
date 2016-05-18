using DailyDotaGod.Models.DailyDotaProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.ViewModels
{
    class DailyDotaLoader: NotificationBase
    {
        private DailyDotaClient Client = DailyDotaClient.Instance;

        private bool _isConnected = false;
        public bool InternetConnected
        {
            get
            {
                return _isConnected;
            }

            set
            {
                SetProperty(ref _isConnected, value);
            }
        }

        private bool _connectionChecked  = false;
        public bool ConnectionChecked
        {
            get
            {
                return _connectionChecked;
            }

            set
            {
                SetProperty(ref _connectionChecked, value);
            }
        }



        public DailyDotaLoader()
        {
            
        }
    }
}
