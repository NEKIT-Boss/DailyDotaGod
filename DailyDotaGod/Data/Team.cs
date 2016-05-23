using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.Data
{
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Tag { get; set; }
        public string Name { get; set; }
        public virtual TeamImage Logo { get; set; }
        public virtual CountryImage CountryLogo { get; set; }
    }
}
