using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class PurchasingManagerIndex
    {
        public int ID { get; set; }
        public Industry Industry { get; set; }
        public Year Year { get; set; }
        public Month Month { get; set; }
        public double PMI { get; set; }
        public double CompositeIndex { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
