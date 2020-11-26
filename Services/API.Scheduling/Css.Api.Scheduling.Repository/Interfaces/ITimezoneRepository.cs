using Css.Api.Core.Models.Domain;
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
        Task<PagedList<Entity>> GetTimezones(TimezoneQueryParameters timezoneQueryParameters);
    }
}