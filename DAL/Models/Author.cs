using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Author : BaseEntity
    {

        public Author()
        {
            Books = new HashSet<Book>();
        }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; }

    }
}
