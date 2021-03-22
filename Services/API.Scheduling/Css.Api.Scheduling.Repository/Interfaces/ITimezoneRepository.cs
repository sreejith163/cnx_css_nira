using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface ITimezoneRepository
    {
        /// <summary>
        /// Gets the timezones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        Task<PagedList<Entity>> GetTimeZones(TimezoneQueryParameters timezoneQueryParameters);

        /// <summary>Gets the time zone.</summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Timezone> GetTimeZone(TimezoneIdDetails timezoneIdDetails);

        /// <summary>Gets the timezone count.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> GetTimezoneCount();

        /// <summary>
        /// Creates the timezone.
        /// </summary>
        /// <param name="timezone">The timezone.</param>
        void CreateTimeZone(Timezone timezone);

        /// <summary>
        /// Updates the timezone.
        /// </summary>
        /// <param name="timezone">The timezone.</param>
        void UpdateTimeZone(Timezone timezone);

        /// <summary>
        /// Deletes the timezone.
        /// </summary>
        /// <param name="timezone">The timezone.</param>
        void DeleteTimeZone(Timezone timezone);
    }
}