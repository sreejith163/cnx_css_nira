using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Job.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Job.Repository
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
        /// A method that fetches all the timezones existing in the system
        /// </summary>
        /// <returns>A list of instances of Timezone</returns>
        public async Task<List<Timezone>> GetTimezones()
        {
            var query = Builders<Timezone>.Filter.Gt(i => i.TimezoneId , 0);
            var timezones = FilterBy(query);
            return await Task.FromResult(timezones.ToList());
        }
    }
}
