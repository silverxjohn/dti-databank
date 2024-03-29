﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class ColumnValue
    {
        public int ID { get; set; }
        public int ColumnID { get; set; }
        public Column Column { get; set; }
        public string Value { get; set; }
        public int RowId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
