using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyDotaGod.Data
{
    public class LeagueImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public byte[] Data { get; set; }
    }
}