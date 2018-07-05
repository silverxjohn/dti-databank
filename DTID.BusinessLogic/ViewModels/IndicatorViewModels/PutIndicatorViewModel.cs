using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTID.BusinessLogic.ViewModels.IndicatorViewModels
{
    public class PutIndicatorViewModel
    {
        public int ID { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }
}
