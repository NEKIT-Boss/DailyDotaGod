﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.Data
{
    public class Team
    {
        public int Id { get; set; }

        public string Tag { get; set; }
        public string Name { get; set; }
        public TeamImage Logo { get; set; }
    }
}
