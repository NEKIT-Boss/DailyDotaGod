using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyDotaGod.Data
{
    public class TeamImage : RawImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}