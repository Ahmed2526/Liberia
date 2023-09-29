using DAL.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Governorate : BaseEntity
    {
        [MaxLength(100)]
        public string NameEn { get; set; } = null!;

        [MaxLength(100)]
        public string NameAr { get; set; } = null!;

        public ICollection<Area> Areas { get; set; } = new List<Area>();

    }
}
