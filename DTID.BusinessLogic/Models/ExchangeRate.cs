using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class ExchangeRate
    {
        public int ID { get; set; }
        public int YearId { get; set; }
        //[ForeignKey("YearId")]
        public Year Year { get; set; }
        public int? MonthID { get; set; }
        //[ForeignKey("MonthId")]
        public Month Month { get; set; }
        public Double Rate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
