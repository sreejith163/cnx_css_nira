using Css.Api.Scheduling.Models.DTO.Response.SchedulingCodeIcon;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface ISchedulingCodeIconRepository
    {
        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        /// <returns></returns>
        Task<List<SchedulingCodeIconDTO>> GetSchedulingCodeIcons();
    }
}
