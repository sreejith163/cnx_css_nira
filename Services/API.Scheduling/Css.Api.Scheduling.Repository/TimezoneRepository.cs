using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class TimezoneRepository : MongoRepository<Timezone>, ITimezoneRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneRepository" /> class.
        /// </summary>
        /// <param name="mongoDbSettings">The mongo database settings.</param>
        public TimezoneRepository(IMongoDbSettings mongoDbSettings): base(mongoDbSettings)
        {
        }

        /// <summary>
        /// Gets the timezones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        public async Task<PagedList<Entity>> GetTimezones(TimezoneQueryParameters timezoneQueryParameters)
        {
            var timeZones = AsQueryable();

            var filteredTimeZones = FilterTimezones(timeZones, timezoneQueryParameters.SearchKeyword);

            var sortedTimeZones = SortHelper.ApplySort(filteredTimeZones, timezoneQueryParameters.OrderBy);

            var pagedTimeZones = sortedTimeZones
                .Skip((timezoneQueryParameters.PageNumber - 1) * timezoneQueryParameters.PageSize)
                .Take(timezoneQueryParameters.PageSize);

            var shapedSkillTags = DataShaper.ShapeData(pagedTimeZones, timezoneQueryParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSkillTags, filteredTimeZones.Count(), timezoneQueryParameters.PageNumber, timezoneQueryParameters.PageSize);
        }

        /// <summary>
        /// Filters the timezones.
        /// </summary>
        /// <param name="timezones">The timezones.</param>
        /// <param name="timeZoneNam">The time zone nam.</param>
        /// <param name="">The .</param>
        /// <returns></returns>
        private IQueryable<Timezone> FilterTimezones(IQueryable<Timezone> timezones, string timeZoneName)
        {
            if (!timezones.Any())
            {
                return timezones;
            }

            if (!string.IsNullOrWhiteSpace(timeZoneName))
            {
                timezones = timezones.Where(o => o.Name.ToLower().Contains(timeZoneName.Trim().ToLower()));
            }

            return timezones;
        }
    }
}
