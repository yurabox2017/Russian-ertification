﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RussianСertification.Models
{
    public class Phone
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
    }
}
