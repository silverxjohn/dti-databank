using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTID.BusinessLogic.Models
{
    public class Indicator
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SourceFile File { get; set; }
        public Directory Parent { get; set; }
        public List<Category> Categories { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
