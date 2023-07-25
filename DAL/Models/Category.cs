﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Category : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

    }
}
