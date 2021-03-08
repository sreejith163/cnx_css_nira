using Css.Api.Scheduling.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{
    public class ForecastScreenRequest
    {
        public long ForecastId { get; set; }
        public int SkillGroupId { get; set; }

        public string Date { get; set; }
        public List<ForecastDataAtrribute> ForecastData { get; set; }
    }
}
