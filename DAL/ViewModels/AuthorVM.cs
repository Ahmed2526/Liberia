using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class AuthorVM
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        [MinLength(3, ErrorMessage = "Minimum Length is 3 char")]
        [Remote("checkUnique", "Authors", AdditionalFields = "Id", ErrorMessage = "Author Already Exist!")]
        public string Name { get; set; } = string.Empty;
    }
}
