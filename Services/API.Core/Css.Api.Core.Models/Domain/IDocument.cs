using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Core.Models.Domain
{
    public interface IDocument
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        /// <summary>
        /// Gets the created at.
        /// </summary>
        DateTime CreatedAt { get; }
    }
}
