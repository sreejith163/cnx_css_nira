using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Repository.DatabaseContext;
using Css.Api.Setup.Repository.Interfaces;
using System.Collections.Generic;

namespace Css.Api.Setup.Repository
{
    public class OperationHourRepository : GenericRepository<OperationHour>, IOperationHourRepository
    {
        /// <summary>Initializes a new instance of the <see cref="OperationHourRepository" /> class.</summary>
        /// <param name="repositoryContext">The repository context.</param>
        public OperationHourRepository(
                    SetupContext repositoryContext)
            : base(repositoryContext)
        {
        }

        /// <summary>
        /// Removes the scheduling type codes.
        /// </summary>
        /// <param name="schedulingTypeCode">The scheduling type code.</param>
        public void RemoveOperatingHours(List<OperationHour> operatingHours)
        {
            DeleteRange(operatingHours);
        }
    }
}
