using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IMongoContext : IDisposable
    {
        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="func">The function.</param>
        void AddCommand(Func<Task> func);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChanges();

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
