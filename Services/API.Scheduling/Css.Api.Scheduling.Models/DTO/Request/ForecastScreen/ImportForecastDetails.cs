using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{
    public class ImportForecastDetails
    {
        //public List<ForecastScreenRequest> ForecastScreens { get; set; }
        public int SkillGroupId { get; set; }

        public List<ImportForecastScreenDataDetails> ForecastScreenDataDetails { get; set; }
    }
}
