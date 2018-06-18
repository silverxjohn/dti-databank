using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.BalanceOfPaymentViewModels
{
    public class QuarterViewModel
    {
        public int ID { get; set; }
        public int? QuarterId { get; set; }
        public string Name { get; set; }
        public double BalanceOfPayments { get; set; }
    }
}
