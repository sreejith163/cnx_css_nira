using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface ITimezoneService
    {
        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <returns></returns>
        Task<CSSResponse> GetTimezones();
    }
}