using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;
using Css.Api.Scheduling.Models.DTO.Request.TimeOff;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Repository
{
    public class TimeOffRepository : GenericRepository<TimeOff>, ITimeOffRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffRepository" /> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        public TimeOffRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        /// <summary>
        /// Gets the timezones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        public async Task<PagedList<Entity>> GetTimeOffs(TimeOffQueryparameter timeOffQueryparameter)
        {
            var timeOffs = FilterBy(x => x.IsDeleted == false);

            var filteredTimeOffs = FilterTimeOffss(timeOffs, timeOffQueryparameter);

            var sortedTimeOffs = SortHelper.ApplySort(filteredTimeOffs, timeOffQueryparameter.OrderBy);

            var pagedTimeOffs = sortedTimeOffs;

            if (!timeOffQueryparameter.SkipPageSize)
            {
                pagedTimeOffs = sortedTimeOffs
                   .Skip((timeOffQueryparameter.PageNumber - 1) * timeOffQueryparameter.PageSize)
                   .Take(timeOffQueryparameter.PageSize);
            }

            var shapedTimeOffs = DataShaper.ShapeData(pagedTimeOffs, timeOffQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedTimeOffs, filteredTimeOffs.Count(), timeOffQueryparameter.PageNumber, timeOffQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the timezone.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <returns></returns>
        public async Task<TimeOff> GetTimeOff(TimeOffIdDetails timeOffIdDetails)
        {
            var query =
                Builders<TimeOff>.Filter.Eq(i => i.Id, new ObjectId(timeOffIdDetails.TimeOffId)) &
                Builders<TimeOff>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets all time offs by description.
        /// </summary>
        /// <param name="timeOffNameDetails">The time off name details.</param>
        /// <returns></returns>
        public async Task<TimeOff> GetAllTimeOffsByDescription(TimeOffNameDetails timeOffNameDetails)
        {
            var query =
                Builders<TimeOff>.Filter.Eq(i => i.Description.ToLowerInvariant(), timeOffNameDetails.Description.ToLowerInvariant()) &
                Builders<TimeOff>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Creates the time off.
        /// </summary>
        /// <param name="timeOff">The time off.</param>
        /// <returns></returns>
        public void CreateTimeOff(TimeOff timeOff)
        {
            InsertOneAsync(timeOff);
        }

        /// <summary>
        /// Updates the time off.
        /// </summary>
        /// <param name="timeOff">The time off.</param>
        /// <returns></returns>
        public void UpdateTimeOff(TimeOff timeOff)
        {
            ReplaceOneAsync(timeOff);
        }

        /// <summary>
        /// Deletes the time off.
        /// </summary>
        /// <param name="timeOff">The time off.</param>
        /// <returns></returns>
        public void DeleteTimeOff(TimeOff timeOff)
        {
            DeleteOneAsync(x => x.Id == timeOff.Id);
        }

        /// <summary>
        /// Filters the time offss.
        /// </summary>
        /// <param name="timeOffs">The time offs.</param>
        /// <param name="timeOffQueryparametere">The time off queryparametere.</param>
        /// <returns></returns>
        private IQueryable<TimeOff> FilterTimeOffss(IQueryable<TimeOff> timeOffs, TimeOffQueryparameter timeOffQueryparametere)
        {
            if (!timeOffs.Any())
            {
                return timeOffs;
            }

            if (!string.IsNullOrWhiteSpace(timeOffQueryparametere.SearchKeyword))
            {
                timeOffs = timeOffs.Where(o => string.Equals(o.Description, timeOffQueryparametere.SearchKeyword, StringComparison.OrdinalIgnoreCase));
            }

            return timeOffs;
        }
    }
}
