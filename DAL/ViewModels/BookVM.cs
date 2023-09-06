using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = null!;
        public string Publisher { get; set; } = string.Empty;
        public DateTime PublishingDate { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ThumbNail { get; set; } = string.Empty;
        public string Hall { get; set; } = string.Empty;
        public bool IsAvailableForRental { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
