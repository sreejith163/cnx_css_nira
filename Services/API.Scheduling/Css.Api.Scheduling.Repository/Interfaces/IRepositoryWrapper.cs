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
        /// Gets the scheduling code icons.
        /// </summary>
        ISchedulingCodeIconRepository SchedulingCodeIcons { get; }

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        ISchedulingCodeTypeRepository SchedulingCodeTypes { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
