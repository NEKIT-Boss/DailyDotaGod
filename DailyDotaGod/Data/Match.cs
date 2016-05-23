using DailyDotaGod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.Data
{
    class Match
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public LiveStatus LiveStatus { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
        public int BestOf { get; set; }
        public virtual League League { get; set; }
    }
}
