using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class ExchangeRate
    {
        public int ID { get; set; }
        public Year Year { get; set; }
        public int? MonthID { get; set; }
        public Month Month { get; set; }
        public double Rate { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
