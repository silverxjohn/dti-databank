using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class BoardOfInvestment
    {
        public int ID { get; set; }
        public int YearID { get; set; }
        public Year Year { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
