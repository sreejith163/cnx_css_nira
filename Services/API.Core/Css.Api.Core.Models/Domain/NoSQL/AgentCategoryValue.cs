﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    public class AgentCategoryValue
    {
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category value.
        /// </summary>
        public string CategoryValue { get; set; }
    }
}
