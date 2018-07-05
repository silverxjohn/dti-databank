using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.DynamicViewModels
{
    public class UploadDynamicViewModel
    {
        public IFormFile File { get; set; }
        public int FolderId { get; set; }
    }
}
