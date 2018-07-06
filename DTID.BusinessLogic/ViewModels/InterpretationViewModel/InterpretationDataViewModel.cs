using DTID.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.InterpretationViewModel
{
    public class InterpretationDataViewModel
    {
        public int ID { get; set; }
        public IFormFile File { get; set; }
        public int CannedId { get; set; }
        public string Message { get; set; }
    }
}
