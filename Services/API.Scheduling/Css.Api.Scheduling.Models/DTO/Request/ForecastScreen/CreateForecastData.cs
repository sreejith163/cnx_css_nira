using Css.Api.Core.Models.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{
    public class CreateForecastData 
    {


        public long ForecastId { get; set; }
        public int SkillGroupId { get; set; }

        
        public string Date { get; set; }
        public List<ForecastDataAtrribute> ForecastData { get; set; }
    }
}
