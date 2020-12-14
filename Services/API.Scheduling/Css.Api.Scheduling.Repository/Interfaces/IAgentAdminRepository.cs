using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Agent admin
    /// </summary>
    public interface IAgentAdminRepository
    {
        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter);

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        Task<Agent> GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails);

        /// <summary>Gets the agent admin ids by employee identifier.</summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Agent> GetAgentAdminIdsByEmployeeIdAndSso(EmployeeIdDetails agentAdminEmployeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails);

        /// <summary>
        /// Gets the agent admin ids by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        Task<Agent> GetAgentAdminIdsByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails);

        /// <summary>Gets the agent admins count.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> GetAgentAdminsCount();

        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        void CreateAgentAdmin(Agent agentAdminRequest);

        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        void UpdateAgentAdmin(Agent agentAdminRequest);

        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        void DeleteAgentAdmin(Agent agentAdminRequest);
    }
}

