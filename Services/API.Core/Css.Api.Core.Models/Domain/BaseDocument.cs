using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Core.Models.Domain
{
    public abstract class BaseDocument : IBaseDocument
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets the created at.
        /// </summary>
        public DateTime CreatedAt => Id.CreationTime;
    }

    public interface IBaseDocument
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        ObjectId Id { get; set; }

        /// <summary>
        /// Gets the created at.
        /// </summary>
        DateTime CreatedAt { get; }
    }
}
