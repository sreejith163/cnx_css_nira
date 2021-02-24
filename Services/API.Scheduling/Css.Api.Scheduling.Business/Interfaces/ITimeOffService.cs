using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.TimeOff;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface ITimeOffService
    {
        /// <summary>
        /// Gets the time offs.
        /// </summary>
        /// <param name="timeOffQueryparameter">The time off queryparameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetTimeOffs(TimeOffQueryparameter timeOffQueryparameter);

        /// <summary>
        /// Gets the time off.
        /// </summary>
        /// <param name="timeOffIdDetails">The time off identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetTimeOff(TimeOffIdDetails timeOffIdDetails);

        /// <summary>
        /// Creates the time off.
        /// </summary>
        /// <param name="timeOffDetails">The time off details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateTimeOff(CreateTimeOff timeOffDetails);

        /// <summary>
        /// Updates the time off.
        /// </summary>
        /// <param name="timeOffIdDetails">The time off identifier details.</param>
        /// <param name="timeOffDetails">The time off details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateTimeOff(TimeOffIdDetails timeOffIdDetails, UpdateTimeOff timeOffDetails);

        /// <summary>
        /// Deletes the time off.
        /// </summary>
        /// <param name="timeOffIdDetails">The time off identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteTimeOff(TimeOffIdDetails timeOffIdDetails);
    }
}