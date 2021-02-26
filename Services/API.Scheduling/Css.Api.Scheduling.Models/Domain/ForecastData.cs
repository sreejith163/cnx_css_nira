﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Css.Api.Scheduling.Models.Domain
{
   public class ForecastData
    {

        public string Time { get; set; }



        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public decimal ForecastedContact { get; set; }


        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public decimal Aht { get; set; }


        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public decimal ForecastedReq { get; set; }


        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public decimal ScheduledOpen { get; set; }




    }
}
