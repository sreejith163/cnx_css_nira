using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interface
{
    /// <summary>
    /// Interface for repository wrapper
    /// </summary>
    public interface IRepositoryWrapper
    {
        /// <summary>
        /// Gets the clients.
        /// </summary>
        IClientRepository Clients { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
