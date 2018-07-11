using DTID.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.InterpretationViewModel
{
    public class InterpretationViewModel
    {
        public int? ID { get; set; }
        public int? CannedId { get; set; }
        public string Message { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
    }
}
