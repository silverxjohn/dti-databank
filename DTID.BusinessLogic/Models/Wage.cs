﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class Wage
    {
        public int ID { get; set; }
        public int YearID { get; set; }
        public Year Year { get; set; }
        public int Wages { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
