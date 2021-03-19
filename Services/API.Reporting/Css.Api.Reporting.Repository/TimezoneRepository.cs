using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The repository for timezone collection
    /// </summary>
    public class TimezoneRepository : GenericRepository<Timezone>, ITimezoneRepository
    {
        /// <summary>Initializes a new instance of the <see cref="TimezoneRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public TimezoneRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// The method to fetch the timezone matching the input timezone id
        /// </summary>
        /// <param name="timezoneId"></param>
        /// <returns>The instance of Timezone</returns>
        public async Task<Timezone> GetTimezone(int timezoneId)
        {
            var query = Builders<Timezone>.Filter.Eq(i => i.TimezoneId, timezoneId);
            var timezone = FilterBy(query);
            return await Task.FromResult(timezone.FirstOrDefault());
        }

        /// <summary>
        /// The method to fetch all the timezone matching the input timezone ids
        /// </summary>
        /// <param name="timezoneIds"></param>
        /// <returns>The list of instances of Timezone</returns>
        public async Task<List<Timezone>> GetTimezones(List<int> timezoneIds)
        {
            var query = Builders<Timezone>.Filter.In(i => i.TimezoneId, timezoneIds);
            var timezone = FilterBy(query);
            return await Task.FromResult(timezone.ToList());
        }
    }
}
