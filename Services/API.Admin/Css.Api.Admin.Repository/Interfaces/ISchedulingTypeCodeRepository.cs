using Css.Api.AdminOps.Models.Domain;
using System.Collections.Generic;

namespace Css.Api.AdminOps.Repository.Interfaces
{
    public interface ISchedulingTypeCodeRepository
    {
        /// <summary>
        /// Removes the scheduling type codes.
        /// </summary>
        /// <param name="schedulingTypeCode">The scheduling type code.</param>
        void RemoveSchedulingTypeCodes(List<SchedulingTypeCode> schedulingTypeCode);
    }
}
