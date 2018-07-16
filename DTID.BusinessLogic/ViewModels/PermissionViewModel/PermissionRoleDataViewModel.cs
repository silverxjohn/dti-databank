using DTID.BusinessLogic.Models;
using DTID.BusinessLogic.ViewModels.PermissionViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.RoleViewModel
{
    public class PermissionRoleDataViewModel
    {
        public int ID { get; set; }
        public List<PermissionRoleViewModel> Permissions { get; set; }
    }
}
