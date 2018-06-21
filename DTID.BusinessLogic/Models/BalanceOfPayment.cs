using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class BalanceOfPayment
    {
        public int ID { get; set; }
        public Year Year { get; set; }
        public Month Month { get; set; }
        public int? MonthID { get; set; }
        public Quarter Quarter { get; set; }
        public int? QuarterID { get; set; }
        public int BalanceOfPayments { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
