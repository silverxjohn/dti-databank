using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class Interpretation
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public bool IsApproved { get; set; }
        public int? CannedIndicatorId { get; set; }
        public CannedIndicator CannedIndicator { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
