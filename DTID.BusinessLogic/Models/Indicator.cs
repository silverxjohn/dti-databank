﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTID.BusinessLogic.Models
{
    public class Indicator
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Directory Parent { get; set; }
        public List<Column> Columns { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
