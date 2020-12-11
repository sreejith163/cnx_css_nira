using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IAgentAdminService
    {
        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameters">The agentAdmin query parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameters);

        /// <summary>
        /// Gets the agentAdmin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agentAdmin identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails);

        /// <summary>
        /// Creates the Agent Admin.
        /// </summary>
        /// <param name="agentAdminDetails">The agentAdmin details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateAgentAdmin(CreateAgentAdmin agentAdminDetails);

        /// <summary>
        /// Updates the Agent Admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agentAdmin identifier details.</param>
        /// <param name="agentAdminDetails">The agentAdmin details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentAdmin(AgentAdminIdDetails agentAdminIdDetails, UpdateAgentAdmin agentAdminDetails);

        /// <summary>
        /// Deletes the Agent Admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agentAdmin identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteAgentAdmin(AgentAdminIdDetails agentAdminIdDetails);
    }
}
