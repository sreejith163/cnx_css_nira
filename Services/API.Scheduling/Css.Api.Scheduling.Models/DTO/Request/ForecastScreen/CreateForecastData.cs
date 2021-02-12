using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{
    public class CreateForecastData 
    {
        public int SkillGroupId { get; set; }

        
        public string Date { get; set; }
        public List<ForecastDataAtrribute> ForecastData { get; set; }
    }
}
