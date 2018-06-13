using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class QuarterYear
    {
        public int ID { get; set; }
        public Quarter Quarter { get; set; }
        public Year Year { get; set; }
    }
}
