using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The repository for scheduling code collection
    /// </summary>
    public class SchedulingCodeRepository : GenericRepository<SchedulingCode>, ISchedulingCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeRepository" /> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        public SchedulingCodeRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        /// <summary>
        /// The method to fetch all active scheduling codes
        /// </summary>
        /// <returns>The list of instances of SchedulingCode</returns>
        public async Task<List<SchedulingCode>> GetSchedulingCodes()
        {
            var schedulingCodes = FilterBy(x => !x.IsDeleted);
            return await Task.FromResult(schedulingCodes.ToList());
        }

        /// <summary>
        /// The method to fetch all matching active scheduling codes based on input names
        /// </summary>
        /// <param name="names">The names of scheduling codes</param>
        /// <returns>The list of instances of SchedulingCode</returns>
        public async Task<List<SchedulingCode>> GetSchedulingCodesByNames(List<string> names)
        {
            var schedulingCodes = FilterBy(x => !x.IsDeleted && names.Contains(x.Name));
            return await Task.FromResult(schedulingCodes.ToList());
        }
    }
}
