using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class Log
    {
        public int ID { get; set; }
        public string Action { get; set; }
        public int UserID { get; set; }
        public User Users { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
