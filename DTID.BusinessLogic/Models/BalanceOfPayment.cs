using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class BalanceOfPayment
    {
        public int ID { get; set; }
        public Month Month { get; set; }
        public QuarterYear QuarterYear { get; set; }
        public int BalanceOfPayments { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
