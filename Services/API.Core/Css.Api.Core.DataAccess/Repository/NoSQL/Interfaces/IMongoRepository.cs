using Css.Api.Core.Models.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces
{
    /// <summary>
    /// Interface for generic mongo repository
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public interface IMongoRepository<TDocument> where TDocument : IBaseDocument
    {
        /// <summary>
        /// Ases the queryable.
        /// </summary>
        /// <returns></returns>
        IQueryable<TDocument> AsQueryable();

        /// <summary>
        /// Filters the by.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression);

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        TDocument FindOne(FilterDefinition<TDocument> filterExpression);

        /// <summary>
        /// Finds the one asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        Task<TDocument> FindOneAsync(FilterDefinition<TDocument> filterExpression);

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TDocument FindById(string id);

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<TDocument> FindByIdAsync(string id);

        /// <summary>
        /// Inserts the one.
        /// </summary>
        /// <param name="document">The document.</param>
        void InsertOne(TDocument document);

        /// <summary>
        /// Inserts the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        Task InsertOneAsync(TDocument document);

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="documents">The documents.</param>
        void InsertMany(ICollection<TDocument> documents);

        /// <summary>
        /// Inserts the many asynchronous.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <returns></returns>
        Task InsertManyAsync(ICollection<TDocument> documents);

        /// <summary>
        /// Replaces the one.
        /// </summary>
        /// <param name="document">The document.</param>
        void ReplaceOne(TDocument document);

        /// <summary>
        /// Replaces the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        Task ReplaceOneAsync(TDocument document);

        /// <summary>
        /// Deletes the one.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        /// <summary>
        /// Deletes the one asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void DeleteById(string id);

        /// <summary>
        /// Deletes the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task DeleteByIdAsync(string id);

        /// <summary>
        /// Deletes the many.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        /// <summary>
        /// Deletes the many asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
