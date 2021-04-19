using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Agent schedule
    /// </summary>
    public interface IAgentScheduleRepository
    {
        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter);

        /// <summary>
        /// Gets the agent schedules by list of employee Ids.
        /// </summary>
        /// <param name="employeeIdList">The list of agent employee id.</param>
        /// <returns></returns>
        Task<List<AgentSchedule>> GetAgentSchedulesByEmployeeIdList(List<string> employeeIdList);

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails);

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange, SchedulingStatus status);

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange);

        /// <summary>
        /// Gets the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        Task<AgentScheduleRange> GetAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange);
        /// <summary>
        /// Gets the agent schedule range.
        /// </summary>
        /// <param name="releaseRangeDetails">The agent schedule identifier details.</param>
     
        /// <returns></returns>
        Task<List<AgentScheduleRange>> GetAgentScheduleRangeForRelease(ReleaseRangeDetails releaseRangeDetails,  string employeeId);
        
        /// <summary>
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier details].
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        Task<bool> IsAgentScheduleRangeExist(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange);


        Task<List<AgentSchedule>> CheckAgentScheduleRangeExist(int asg);

        /// <summary>
        /// Gets the agent schedule count.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<long> GetAgentScheduleCount(AgentScheduleIdDetails agentScheduleIdDetails);

        /// <summary>
        /// Gets the employee identifier by agent schedule identifier.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<string> GetEmployeeIdByAgentScheduleId(AgentScheduleIdDetails agentScheduleIdDetails);

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<List<string>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>
        /// Gets the agent schedule by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        Task<AgentSchedule> GetAgentScheduleByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails);

        /// <summary>
        /// Gets the agent schedule count by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        Task<long> GetAgentScheduleCountByEmployeeId(EmployeeIdDetails employeeIdDetails);

        /// <summary>
        /// Gets the agent scheduling group export.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<List<AgentSchedule>> GetAgentSchedulingGroupExport(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);


        Task<List<AgentSchedule>> GetEmployeeScheduleExport(string employeeId);


        Task<List<AgentSchedule>> GetDateRange(List<int> asgList);


        /// <summary>
        /// Creates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        void CreateAgentSchedule(AgentSchedule agentScheduleRequest);

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        void UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails);


        //void UpdateAgentScheduleRelease(UpdateAgentSchedule agentScheduleDetails);
        
        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleEmployeeDetails">The update agent schedule employee details.</param>
        void UpdateAgentSchedule(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails);

        /// <summary>Updates the agent schedule with ranges.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleEmployeeDetails">The update agent schedule employee details.</param>
        void UpdateAgentScheduleWithRanges(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails);

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        void UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleRange agentScheduleRange, ModifiedUserDetails modifiedUserDetails);

        /// <summary>
        /// Copies the agent schedules.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        void CopyAgentSchedules(EmployeeIdDetails employeeIdDetails, AgentScheduleRange agentScheduleRange);

        /// <summary>
        /// Copies the multiple agent schedules.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        void MultipleCopyAgentScheduleChart(EmployeeIdDetails employeeIdDetails, AgentScheduleRange agentScheduleRange);

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        void UpdateAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRange);

        /// <summary>
        /// Removes the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        void DeleteAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange);

        

        void UpdateAgentScheduleRangeRelease(string employeeId,BatchRelease agentScheduleDetails);
        void DeleteAgentScheduleRangeImport(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange);

        /// <summary>
        /// Deletes the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        void DeleteAgentSchedule(EmployeeIdDetails employeeIdDetails);

        Task<List<AgentSchedule>> GetAgentScheduleIdForRelease(ReleaseRangeDetails releaseRangeDetails);
    }
}

