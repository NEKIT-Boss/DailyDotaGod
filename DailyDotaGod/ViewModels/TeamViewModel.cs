using DailyDotaGod.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace DailyDotaGod.ViewModels
{
    class TeamViewModel : NotificationBase<Team>
    {
        public TeamViewModel(Team thing) : base(thing)
        {
        }

        public string Name
        {
            get
            {
                return This.Name;
            }
        }

        public string Tag
        {
            get
            {
                return This.Tag;
            }
        }

        public BitmapImage Logo
        {
            get
            {
                //Add some async here after
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                    {
                        if (This.Logo == null)
                        {
                            return new BitmapImage(new Uri("ms-appx://DailyDotaGod/Assets/UnknownTeam.png"));
                        }

                        writer.WriteBytes(This.Logo.Data);
                        writer.StoreAsync().AsTask().Wait();
                    }
                    BitmapImage image = new BitmapImage();
                    image.SetSource(stream);
                    return image;
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
