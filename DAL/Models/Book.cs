using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Book : BaseEntity
    {
        public Book()
        {
            Author = new Author();
            Category = new Category();
        }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public Author Author { get; set; }
        public Category Category { get; set; }
    }
}
