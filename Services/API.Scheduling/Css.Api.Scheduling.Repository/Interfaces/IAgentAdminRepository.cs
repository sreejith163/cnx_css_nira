using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using MongoDB.Bson;
using System.Collections.Generic;
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

        /// <summary>Gets the agent admin by employee identifier.</summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Agent> GetAgentAdminIdsByEmployeeIdAndSso(EmployeeIdDetails agentAdminEmployeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails);

        /// <summary>Gets the agent admin by sso.</summary>
        /// <param name="agentAdminSsoDetails">The agent admin sso details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Agent> GetAgentAdminBySso(AgentAdminSsoDetails agentAdminSsoDetails);

        /// <summary>
        /// Gets the agent admin by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        Task<Agent> GetAgentAdminByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails);

        /// <summary>
        /// Gets the agent admins by employee ids.
        /// </summary>
        /// <param name="agentAdminEmployeeIdsDetails">The agent admin employee ids details.</param>
        /// <returns></returns>
        Task<List<Agent>> GetAgentAdminsByEmployeeIds(List<int> agentAdminEmployeeIdsDetails);

        /// <summary>
        /// Gets the agent admins by category identifier.
        /// </summary>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        Task<List<Agent>> GetAgentAdminsByCategoryId(List<int> agentCategoryDetails);

        /// <summary>Gets the agent admins by ids.</summary>
        /// <param name="agentAdminIdsDetails">The agent admin ids details.</param>
        /// <param name="sourceSchedulingGroupId">The source scheduling group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<Agent>> GetAgentAdminsByIds(List<ObjectId> agentAdminIdsDetails, int sourceSchedulingGroupId);

        /// <summary>
        /// Gets the employee ids by agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<List<int>> GetEmployeeIdsByAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>Gets the agent admins count.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> GetAgentAdminsCount();

        /// <summary>
        /// Updates the agent category values.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="categoryValue">The category valu.</param>
        void UpdateAgentCategoryValue(EmployeeIdDetails employeeIdDetails, AgentCategoryValue categoryValue);

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

