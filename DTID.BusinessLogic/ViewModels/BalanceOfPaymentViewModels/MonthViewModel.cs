using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.BalanceOfPaymentViewModels
{
    public class MonthViewModel
    {
        public int ID { get; set; }
        public int? MonthId { get; set; }
        public string Name { get; set; }
        public double BalanceOfPayments { get; set; }
    }
}
