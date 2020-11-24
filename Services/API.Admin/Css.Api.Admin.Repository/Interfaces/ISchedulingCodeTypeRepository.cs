using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ISchedulingCodeTypeRepository
    {
        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetSchedulingCodeTypes();
    }
}
