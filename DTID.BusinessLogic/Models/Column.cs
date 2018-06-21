using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class Column
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ColumnType Type { get; set; }
        public Category Category { get; set; }
        public List<ColumnValues> Values { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
