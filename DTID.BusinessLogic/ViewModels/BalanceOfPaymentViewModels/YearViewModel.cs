using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.BalanceOfPaymentViewModels
{
    public class YearViewModel
    {
        public int ID { get; set; }
        public int YearId { get; set; }
        public string Name { get; set; }
        public double BalanceOfPayments { get; set; }
        public List<MonthViewModel> Months { get; set; }
        public List<QuarterViewModel> Quarters { get; set; }
    }
}
