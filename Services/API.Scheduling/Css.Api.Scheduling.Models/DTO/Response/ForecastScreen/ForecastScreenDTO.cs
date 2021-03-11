using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Response.ForecastScreen
{
   public class ForecastScreenDTO
    {

      
 


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SkillGroupId { get; set; }
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        /// 


        public string Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public List<ForecastData> ForecastData { get; set; }
    }

}
