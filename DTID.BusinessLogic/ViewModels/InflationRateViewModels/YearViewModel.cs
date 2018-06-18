using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.InflationRateViewModels
{
    public class YearViewModel
    {
        public int ID { get; set; }
        public int YearId { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public List<MonthViewModel> Months { get; set; }
    }
}
