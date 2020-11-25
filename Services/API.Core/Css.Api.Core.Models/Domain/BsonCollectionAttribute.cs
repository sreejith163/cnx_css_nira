using System;

namespace Css.Api.Core.Models.Domain
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonCollectionAttribute"/> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
