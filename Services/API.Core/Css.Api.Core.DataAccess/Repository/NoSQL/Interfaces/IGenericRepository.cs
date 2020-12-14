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
    public interface IGenericRepository<TDocument> : IDisposable where TDocument : IBaseDocument
    {
        /// <summary>
        /// Ases the queryable.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IQueryable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filter);

        /// <summary>
        /// Filters the by.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filter);

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<TDocument> FindByIdAsync(FilterDefinition<TDocument> filter);

        /// <summary>
        /// Finds the count by identifier asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<long> FindCountByIdAsync(FilterDefinition<TDocument> filter);

        /// <summary>
        /// Inserts the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        void InsertOneAsync(TDocument document);

        /// <summary>
        /// Inserts the many asynchronous.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <returns></returns>
        void InsertManyAsync(ICollection<TDocument> documents);

        /// <summary>
        /// Replaces the one asynchronous.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        void ReplaceOneAsync(TDocument document);

        /// <summary>
        /// Updates the one asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        void UpdateOneAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update);

        /// <summary>
        /// Updates the many asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        void UpdateManyAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update);

        /// <summary>
        /// Deletes the one asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        void DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        /// <summary>
        /// Deletes the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        void DeleteByIdAsync(string id);

        /// <summary>
        /// Deletes the many asynchronous.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <returns></returns>
        void DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
