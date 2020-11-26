using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface ITimezoneService
    {
        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetTimeZones(TimezoneQueryParameters timezoneQueryParameters);

        /// <summary>
        /// Gets the timezone.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetTimeZone(TimezoneIdDetails timezoneIdDetails);

        /// <summary>
        /// Creates the timezone.
        /// </summary>
        /// <param name="timezoneDetails">The timezone details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateTimeZone(CreateTimezone timezoneDetails);

        /// <summary>
        /// Updates the timezone.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <param name="timezoneDetails">The timezone details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateTimeZone(TimezoneIdDetails timezoneIdDetails, UpdateTimezone timezoneDetails);

        /// <summary>
        /// Deletes the timezone.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteTimeZone(TimezoneIdDetails timezoneIdDetails);
    }
}