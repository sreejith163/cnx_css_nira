using Css.Api.Scheduling.Models.DTO.Response.SchedulingCodeIcon;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface ISchedulingCodeTypeRepository
    {
        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        /// <returns></returns>
        Task<List<SchedulingCodeTypeDTO>> GetSchedulingCodeTypes();
    }
}
