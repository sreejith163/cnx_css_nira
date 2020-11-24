using System.Threading.Tasks;

namespace Css.Api.AdminOps.Repository.Interfaces
{
    /// <summary>
    /// Interface for repository wrapper
    /// </summary>
    public interface IRepositoryWrapper
    { 
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
        /// Gets the scheduling type codes.
        /// </summary>
        ISchedulingTypeCodeRepository SchedulingTypeCodes { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
