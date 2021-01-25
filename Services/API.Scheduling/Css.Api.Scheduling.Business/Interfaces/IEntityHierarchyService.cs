using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IEntityHierarchyService
    {
        /// <summary>Gets the entity hierarchy.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetEntityHierarchy(ClientIdDetails clientIdDetails);
    }
}
