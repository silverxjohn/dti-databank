using DTID.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.IndicatorViewModels
{
    public class IndicatorViewModel
    {
        public List<Indicator> Root { get; set; }
        public List<Indicator> Macroeconomics { get; set; }
        public List<Indicator> Investments { get; set; }
        public List<Indicator> Others { get; set; }

        public IndicatorViewModel(List<Indicator> root, List<Indicator> macroeconomics, List<Indicator> investments, List<Indicator> others)
        {
            Root = root;
            Macroeconomics = macroeconomics;
            Investments = investments;
            Others = others;
        }
    }
}
