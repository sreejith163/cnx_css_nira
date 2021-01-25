using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Scheduling Code
    /// </summary>
    public interface ISchedulingCodeRepository
    {
        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeQueryparameter">The scheduling code queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetSchedulingCodes(SchedulingCodeQueryparameter schedulingCodeQueryparameter);

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<SchedulingCode> GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails);

        /// <summary>
        /// Gets the scheduling codes count by ids.
        /// </summary>
        /// <param name="schedulingCodes">The scheduling codes.</param>
        /// <returns></returns>
        Task<long> GetSchedulingCodesCountByIds(List<int> schedulingCodes);

        /// <summary>
        /// Gets the scheduling codes count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetSchedulingCodesCount();

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeRequest">The scheduling code request.</param>
        void CreateSchedulingCode(SchedulingCode schedulingCodeRequest);

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeRequest">The scheduling code request.</param>
        void UpdateSchedulingCode(SchedulingCode schedulingCodeRequest);
    }
}

