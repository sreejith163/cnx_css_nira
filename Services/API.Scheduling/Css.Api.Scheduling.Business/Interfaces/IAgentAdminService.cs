﻿using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
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
        /// Gets the agent admin by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentAdminByEmployeeId(EmployeeIdDetails employeeIdDetails);

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

        /// <summary>Moves the agent admins.</summary>
        /// <param name="moveAgentAdminsDetails">The move agent admins details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> MoveAgentAdmins(MoveAgentAdminsDetails moveAgentAdminsDetails);

        /// <summary>
        /// Updates the agent category values.
        /// </summary>
        /// <param name="agentCategoryValue">The agent category value.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentCategoryValues(CreateAgentCategoryValue agentCategoryValue);

        /// <summary>
        /// Deletes the Agent Admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agentAdmin identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteAgentAdmin(AgentAdminIdDetails agentAdminIdDetails);

        /// <summary>Creates the agent activity log.</summary>
        /// <param name="agentActivityLogDetails">The agent activity log details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> CreateAgentActivityLog(CreateAgentActivityLog agentActivityLogDetails);

        /// <summary>Gets the agent activity logs.</summary>
        /// <param name="agentActivityLogQueryParameter">The agent activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetAgentActivityLogs(ActivityLogQueryParameter agentActivityLogQueryParameter);
    }
}
