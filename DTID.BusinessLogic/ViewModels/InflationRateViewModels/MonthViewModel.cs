using DTID.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.InflationRateViewModels
{
    public class MonthViewModel
    {
        public int ID { get; set; }
        public int MonthId { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
    }
}
