using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class SourceFile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
