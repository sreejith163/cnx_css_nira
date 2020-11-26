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
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IBaseDocument
    {
        /// <summary>
        /// The client
        /// </summary>
        private readonly IMongoClient _client;

        /// <summary>
        /// The collection
        /// </summary>
        private readonly IMongoCollection<TDocument> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TDocument}"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public MongoRepository(IMongoDbSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            var database = _client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        /// <summary>
        /// Ases the queryable.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        /// <summary>
        /// Filters the by.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        public virtual IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        public virtual TDocument FindOne(FilterDefinition<TDocument> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        /// <summary>
        /// Finds the one asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        public virtual async Task<TDocument> FindOneAsync(FilterDefinition<TDocument> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<TDocument> FindByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Inserts the one.
        /// </summary>
        /// <param name="document">The document.</param>
        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        /// <summary>
        /// Inserts the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public virtual async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="documents">The documents.</param>
        public void InsertMany(ICollection<TDocument> documents)
        {
            using var session = _client.StartSession();
            try
            {
                session.StartTransaction();
                _collection.InsertMany(session, documents);
            }
            catch (Exception ex)
            {
                session.AbortTransactionAsync();
                throw ex;
            }

            session.CommitTransactionAsync();
        }

        /// <summary>
        /// Inserts the many asynchronous.
        /// </summary>
        /// <param name="documents">The documents.</param>
        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            using var session = await _client.StartSessionAsync();
            try
            {
                session.StartTransaction();
                await _collection.InsertManyAsync(session, documents);
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw ex;
            }

            await session.CommitTransactionAsync();
        }

        /// <summary>
        /// Replaces the one.
        /// </summary>
        /// <param name="document">The document.</param>
        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        /// <summary>
        /// Replaces the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        /// <summary>
        /// Deletes the one.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        /// <summary>
        /// Deletes the one asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        /// <summary>
        /// Deletes the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task DeleteByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            using var session = _client.StartSession();
            try
            {
                session.StartTransaction();
                _collection.DeleteMany(filterExpression);
            }
            catch (Exception ex)
            {
                session.AbortTransactionAsync();
                throw ex;
            }

            session.CommitTransactionAsync();
        }

        /// <summary>
        /// Deletes the many asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        public async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            using var session = await _client.StartSessionAsync();
            try
            {
                session.StartTransaction();
                await _collection.DeleteManyAsync(filterExpression);
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw ex;
            }

            await session.CommitTransactionAsync();
        }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault())
                ?.CollectionName;
        }
    }
}
