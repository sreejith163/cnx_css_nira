
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{

    public class ForecastDataAtrribute
    {
     
        public string Time { get; set; }



      
        public long ForecastedContact { get; set; }


        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public decimal Aht { get; set; }


        
        public long ForecastedReq { get; set; }




    }

}
