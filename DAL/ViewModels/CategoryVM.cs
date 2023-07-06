using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        [MinLength(3, ErrorMessage = "Minimum Length is 3 char")]
        public string Name { get; set; }
    }
}
