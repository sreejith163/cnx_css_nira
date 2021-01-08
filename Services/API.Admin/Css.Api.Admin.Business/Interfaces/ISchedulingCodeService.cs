using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ISchedulingCodeService
    {
        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeParameters">The scheduling code parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetSchedulingCodes(SchedulingCodeQueryParameters schedulingCodeParameters);

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails);

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeDetails">The scheduling code details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateSchedulingCode(CreateSchedulingCode schedulingCodeDetails);

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <param name="schedulingCodeDetails">The scheduling code details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails, UpdateSchedulingCode schedulingCodeDetails);

        /// <summary>Reverts the scheduling code.</summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <param name="schedulingCodeDetails">The scheduling code details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> RevertSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails, UpdateSchedulingCode schedulingCodeDetails);

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails);
    }
}
