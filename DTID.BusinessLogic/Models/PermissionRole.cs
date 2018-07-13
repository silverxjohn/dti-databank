﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class PermissionRole
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public int PermissionID { get; set; }
        public Permission Permission { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
