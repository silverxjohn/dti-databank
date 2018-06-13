using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class InflationRate
    {
        public int ID { get; set; }
        public Year Year { get; set; }
        public Month Month { get; set; }
        public Double Rate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
