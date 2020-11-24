using Css.Api.SetupMenu.Models.Domain;
using System.Collections.Generic;

namespace Css.Api.SetupMenu.Repository.Interfaces
{
    public interface IOperationHourRepository
    {
        /// <summary>Removes the operating hours.</summary>
        /// <param name="operatingHours">The operating hours.</param>
        void RemoveOperatingHours(List<OperationHour> operatingHours);
    }
}
