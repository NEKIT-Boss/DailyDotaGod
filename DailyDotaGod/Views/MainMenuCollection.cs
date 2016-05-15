using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.Views
{
    class MainMenuItem
    {
        public string Title { get; set; }
        public string Glyph { get; set; }
        public int Position { get; set; }
    }

    class MainMenuCollection : List<MainMenuItem>
    {
        
    }
}
