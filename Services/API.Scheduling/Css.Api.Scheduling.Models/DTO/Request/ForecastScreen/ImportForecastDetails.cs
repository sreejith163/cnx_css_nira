using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{
    public class ImportForecastDetails
    {
 

        public string Date { get; set; }


        public List<ForecastDataAtrribute> ForecastData { get; set; }
    }
}
