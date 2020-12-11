using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Core.Models.Domain
{
    /// <summary>
    /// /Implementation for the base document
    /// </summary>
    /// <seealso cref="Css.Api.Core.Models.Domain.IBaseDocument" />
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

    /// <summary>
    /// Interface for the base document
    /// </summary>
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
