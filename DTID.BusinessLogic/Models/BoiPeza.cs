using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class BoiPeza
    {
        public int ID { get; set; }
        public int YearId { get; set; }
        public Year Year { get; set; }
        public double BOI { get; set; }
        public double Peza { get; set; }
        public double Total {
            get
            {
                return BOI + Peza;
            }
        }
        public bool IsApproved { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
