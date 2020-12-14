using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Css.Api.Core.DataAccess.Repository.NoSQL
{
    /// <summary>
    /// Implementation for generic mongo repository
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <seealso cref="Css.Api.Core.DataAccess.Repository.Interfaces.IMongoRepository{TDocument}" />
    public class GenericRepository<TDocument> : IGenericRepository<TDocument> where TDocument : IBaseDocument
    {
        /// <summary>
        /// The context
        /// </summary>
        protected readonly IMongoContext Context;

        /// <summary>
        /// The database set
        /// </summary>
        protected IMongoCollection<TDocument> Collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public GenericRepository(IMongoContext context)
        {
            Context = context;
            Collection = Context.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        /// <summary>
        /// Ases the queryable.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public virtual IQueryable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filter)
        {
            return Collection.AsQueryable().Where(filter);
        }

        /// <summary>
        /// Filters the by.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public virtual IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filter)
        {
            return Collection.Find(filter).ToEnumerable();
        }

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public virtual async Task<TDocument> FindByIdAsync(FilterDefinition<TDocument> filter)
        {
            return await Collection.Find(filter).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Finds the count by identifier asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public virtual async Task<long> FindCountByIdAsync(FilterDefinition<TDocument> filter)
        {
            return await Collection.CountDocumentsAsync(filter);
        }

        /// <summary>
        /// Inserts the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public virtual void InsertOneAsync(TDocument document)
        {
            Context.AddCommand(() => Collection.InsertOneAsync(document));
        }

        /// <summary>
        /// Inserts the many asynchronous.
        /// </summary>
        /// <param name="documents">The documents.</param>
        public virtual void InsertManyAsync(ICollection<TDocument> documents)
        {
            Context.AddCommand(() => Collection.InsertManyAsync(documents));
        }

        /// <summary>
        /// Replaces the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        public virtual void ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            Context.AddCommand(() => Collection.FindOneAndReplaceAsync(filter, document));
        }

        /// <summary>
        /// Updates the one asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        public virtual void UpdateOneAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update)
        {
            Context.AddCommand(() => Collection.UpdateOneAsync(filter, update));
        }

        /// <summary>
        /// Updates the many asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        public virtual void UpdateManyAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update)
        {
            Context.AddCommand(() => Collection.UpdateManyAsync(filter, update));
        }

        /// <summary>
        /// Deletes the one asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        public virtual void DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            Context.AddCommand(() => Collection.FindOneAndDeleteAsync(filterExpression));
        }

        /// <summary>
        /// Deletes the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual void DeleteByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            Context.AddCommand(() => Collection.FindOneAndDeleteAsync(filter));
        }

        /// <summary>
        /// Deletes the many asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        public virtual void DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            Context.AddCommand(() => Collection.DeleteManyAsync(filterExpression));
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
        }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
        private string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault())
                ?.CollectionName;
        }
    }
}
