using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;

namespace Css.Api.Scheduling.Repository
{
    public class TimezoneRepository : GenericRepository<Timezone>, ITimezoneRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneRepository" /> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        public TimezoneRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        /// <summary>
        /// Gets the timezones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        public async Task<PagedList<Entity>> GetTimeZones(TimezoneQueryParameters timezoneQueryParameters)
        {
            var timeZones = FilterBy(x => x.IsDeleted == false);

            var filteredTimeZones = FilterTimezones(timeZones, timezoneQueryParameters.SearchKeyword);

            var sortedTimeZones = SortHelper.ApplySort(filteredTimeZones, timezoneQueryParameters.OrderBy);

            var pagedTimeZones = sortedTimeZones;

            if (!timezoneQueryParameters.SkipPageSize)
            {
                pagedTimeZones = sortedTimeZones
                   .Skip((timezoneQueryParameters.PageNumber - 1) * timezoneQueryParameters.PageSize)
                   .Take(timezoneQueryParameters.PageSize);
            }

            var shapedSkillTags = DataShaper.ShapeData(pagedTimeZones, timezoneQueryParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSkillTags, filteredTimeZones.Count(), timezoneQueryParameters.PageNumber, timezoneQueryParameters.PageSize);
        }

        /// <summary>
        /// Gets the timezone.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <returns></returns>
        public async Task<Timezone> GetTimeZone(TimezoneIdDetails timezoneIdDetails)
        {
            var query = Builders<Timezone>.Filter.Eq(i => i.TimezoneId, timezoneIdDetails.TimezoneId) 
                & Builders<Timezone>.Filter.Eq(i => i.TimezoneId, timezoneIdDetails.TimezoneId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Creates the timezone.
        /// </summary>
        /// <param name="timezone">The timezone.</param>
        public void CreateTimeZone(Timezone timezone)
        {
            InsertOneAsync(timezone);
        }

        /// <summary>
        /// Updates the timezone.
        /// </summary>
        /// <param name="timezone">The timezone.</param>
        public void UpdateTimeZone(Timezone timezone)
        {
            ReplaceOneAsync(timezone);
        }

        /// <summary>
        /// Deletes the timezone.
        /// </summary>
        /// <param name="timezone">The timezone.</param>
        public void DeleteTimeZone(Timezone timezone)
        {
            DeleteOneAsync(x => x.Id == timezone.Id);
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
                timezones = timezones.Where(o => string.Equals(o.Name, timeZoneName, StringComparison.OrdinalIgnoreCase));
            }

            return timezones;
        }
    }
}
