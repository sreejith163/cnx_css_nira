using Css.Api.Admin.Models.Domain;
using System.Collections.Generic;

namespace Css.Api.Admin.Repository.Interfaces
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
