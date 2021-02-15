using Css.Api.Core.Models.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("forecast_screen")]
    public class ForecastScreen : BaseDocument
    {


        /// <summary>
        /// Gets or sets the date from.
        /// </summary>

    
        public long ForecastId { get; set; }

        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets forecast date.
        /// </summary>
     
        public string Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ForecastData> ForecastData { get; set; }
    }
    }
