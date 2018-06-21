using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class BoardOfInvestment
    {
        public int ID { get; set; }
        public int YearId { get; set; }
        [ForeignKey("YearId")]
        public Year Year { get; set; }
        public Double Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
