using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.TimeOff;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface ITimeOffRepository
    {
        /// <summary>
        /// Gets the time offs.
        /// </summary>
        /// <param name="timeOffQueryparameter">The time off queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetTimeOffs(TimeOffQueryparameter timeOffQueryparameter);

        /// <summary>
        /// Gets the time off.
        /// </summary>
        /// <param name="timeOffIdDetails">The time off identifier details.</param>
        /// <returns></returns>
        Task<TimeOff> GetTimeOff(TimeOffIdDetails timeOffIdDetails);

        /// <summary>
        /// Gets all time offs by description.
        /// </summary>
        /// <param name="timeOffNameDetails">The time off name details.</param>
        /// <returns></returns>
        Task<TimeOff> GetAllTimeOffsByDescription(TimeOffNameDetails timeOffNameDetails);

        /// <summary>
        /// Creates the time off.
        /// </summary>
        /// <param name="timeOff">The time off.</param>
        void CreateTimeOff(TimeOff timeOff);

        /// <summary>
        /// Updates the time off.
        /// </summary>
        /// <param name="timeOff">The time off.</param>
        void UpdateTimeOff(TimeOff timeOff);

        /// <summary>
        /// Deletes the time off.
        /// </summary>
        /// <param name="timeOff">The time off.</param>
        void DeleteTimeOff(TimeOff timeOff);
    }
}