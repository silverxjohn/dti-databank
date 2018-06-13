using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class Directory
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public int? ParentID { get; set; }
        public Directory Parent { get; set; }
        public List<Directory> Directories { get; set; }
        public List<Indicator> Indicators { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
