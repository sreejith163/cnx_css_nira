using Css.Api.Scheduling.Repository.Interfaces;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
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
        /// Gets the client lob groups.
        /// </summary>
        IClientLOBGroupRepository ClientLOBGroups { get; }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        ISchedulingCodeRepository SchedulingCodes { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
