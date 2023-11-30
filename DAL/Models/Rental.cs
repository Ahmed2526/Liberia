using DAL.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Rental : BaseEntity
    {
        public string SubscriberId { get; set; }
        public Subscriber Subscriber { get; set; }
        public DateTime StartDate { get; set; }
        public bool PenaltyPaid { get; set; }

    }
}
