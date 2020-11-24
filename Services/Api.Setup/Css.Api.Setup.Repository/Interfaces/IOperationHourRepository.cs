using Css.Api.Setup.Models.Domain;
using System.Collections.Generic;

namespace Css.Api.Setup.Repository.Interfaces
{
    public interface IOperationHourRepository
    {
        /// <summary>Removes the operating hours.</summary>
        /// <param name="operatingHours">The operating hours.</param>
        void RemoveOperatingHours(List<OperationHour> operatingHours);
    }
}
