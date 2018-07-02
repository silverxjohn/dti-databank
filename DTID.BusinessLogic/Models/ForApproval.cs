using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class ForApproval
    {
        public int ID { get; set; }
        public string Activity { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Comment { get; set; }
        public ApprovalStatus isApproved { get; set; }
        public int? IndicatorID { get; set; }
        public int? CannedIndicatorID { get; set; }
        public CannedIndicator CannedIndicator { get; set; }
    }
}
