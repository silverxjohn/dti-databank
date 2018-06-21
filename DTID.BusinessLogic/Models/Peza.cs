using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
     public class Peza
    {
        public int ID { get; set; }
        public Year Year { get; set; }
        public Double Amount { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
