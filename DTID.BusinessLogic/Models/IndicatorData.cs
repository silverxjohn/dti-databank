using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class IndicatorData
    {
        public int ID { get; set; }
        public Indicator Indicator { get; set; }
        public string Data { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
