using Css.Api.Core.DataAccess.Repository;
using Css.Api.SetupMenu.Models.Domain;
using Css.Api.SetupMenu.Repository.DatabaseContext;
using Css.Api.SetupMenu.Repository.Interfaces;
using System.Collections.Generic;

namespace Css.Api.SetupMenu.Repository
{
    public class OperationHourRepository : GenericRepository<OperationHour>, IOperationHourRepository
    {
        /// <summary>Initializes a new instance of the <see cref="OperationHourRepository" /> class.</summary>
        /// <param name="repositoryContext">The repository context.</param>
        public OperationHourRepository(
                    SetupMenuContext repositoryContext)
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
