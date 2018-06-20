using DTID.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.DirectoryViewModels
{
    public class DirectoryViewModel
    {
        public List<Directory> Directories { get; set; }
        public List<Indicator> Indicators { get; set; } 
    }
}
