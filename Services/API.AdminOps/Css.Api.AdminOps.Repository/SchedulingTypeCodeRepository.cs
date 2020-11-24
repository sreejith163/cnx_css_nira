using Css.Api.Core.DataAccess.Repository;
using Css.Api.AdminOps.Models.Domain;
using Css.Api.AdminOps.Repository.DatabaseContext;
using Css.Api.AdminOps.Repository.Interfaces;
using System.Collections.Generic;

namespace Css.Api.AdminOps.Repository
{
    public class SchedulingTypeCodeRepository : GenericRepository<SchedulingTypeCode>, ISchedulingTypeCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingTypeCodeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="">The .</param>
        public SchedulingTypeCodeRepository(
                    AdminOpsContext repositoryContext)
            : base(repositoryContext)
        {
        }

        /// <summary>
        /// Removes the scheduling type codes.
        /// </summary>
        /// <param name="schedulingTypeCode">The scheduling type code.</param>
        public void RemoveSchedulingTypeCodes(List<SchedulingTypeCode> schedulingTypeCode)
        {
            DeleteRange(schedulingTypeCode);
        }
    }
}
