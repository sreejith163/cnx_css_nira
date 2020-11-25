using MongoDB.Bson;
using System;

namespace Css.Api.Core.Models.Domain
{
    public abstract class Document : IDocument
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
}
