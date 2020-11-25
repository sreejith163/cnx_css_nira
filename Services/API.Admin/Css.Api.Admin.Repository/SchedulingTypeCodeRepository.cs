using Css.Api.Core.DataAccess.Repository;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using System.Collections.Generic;

namespace Css.Api.Admin.Repository
{
    public class SchedulingTypeCodeRepository : EFCoreGenericRepository<SchedulingTypeCode>, ISchedulingTypeCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingTypeCodeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="">The .</param>
        public SchedulingTypeCodeRepository(
                    AdminContext repositoryContext)
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
