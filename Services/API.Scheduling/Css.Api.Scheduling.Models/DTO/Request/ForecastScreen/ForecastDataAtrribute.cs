
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{

    public class ForecastDataAtrribute
    {
     
        public string Time { get; set; }


        public string ForecastedContact { get; set; }



        public string Aht { get; set; }


        public string ForecastedReq { get; set; }


        public string ScheduledOpen { get; set; }
    }

}
