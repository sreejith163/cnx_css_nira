using Css.Api.Scheduling.Models.DTO.Request.ForecastScreen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Response.ForecastScreen
{
    public class ForecastDataResponse
    {
        public List<string> Errors;
        public List<string> Success;
        public string ImportStatus;
        
    }
}
