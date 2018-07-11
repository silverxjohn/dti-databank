using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.IndicatorViewModels
{
    public class AttachmentViewModel
    {
        public int ID { get; set; }
        public string Filename { get; set; }
        public string Mime { get; set; }
        public string HashedName { get; set; }
        public string Extension { get; set; }
        public string NewName { get; set; }
    }
}
