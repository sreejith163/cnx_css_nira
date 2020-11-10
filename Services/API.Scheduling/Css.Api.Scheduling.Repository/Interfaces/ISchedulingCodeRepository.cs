using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface ISchedulingCodeRepository
    {
        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeParameters">The scheduling code parameters.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetSchedulingCodes(SchedulingCodeQueryParameters schedulingCodeParameters);

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<SchedulingCode> GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails);

        /// <summary>
        /// Gets the scheduling codes by description.
        /// </summary>
        /// <param name="schedulingCodeNameDetails">The scheduling code name details.</param>
        /// <returns></returns>
        Task<List<int>> GetSchedulingCodesByDescription(SchedulingCodeNameDetails schedulingCodeNameDetails);

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        void CreateSchedulingCode(SchedulingCode schedulingCode);

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        void UpdateSchedulingCode(SchedulingCode schedulingCode);

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        void DeleteSchedulingCode(SchedulingCode schedulingCode);
    }
}
