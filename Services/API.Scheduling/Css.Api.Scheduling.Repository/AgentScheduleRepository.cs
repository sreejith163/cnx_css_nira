using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentScheduleRepository : GenericRepository<AgentSchedule>, IAgentScheduleRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="AgentScheduleRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentScheduleRepository(
            IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedules = FilterBy(x => true);

            var filteredAgentSchedules = FilterAgentSchedules(agentSchedules, agentScheduleQueryparameter);

            var sortedAgentSchedules = SortHelper.ApplySort(filteredAgentSchedules, agentScheduleQueryparameter.OrderBy);

            var pagedAgentSchedules = sortedAgentSchedules
                .Skip((agentScheduleQueryparameter.PageNumber - 1) * agentScheduleQueryparameter.PageSize)
                .Take(agentScheduleQueryparameter.PageSize);

            var shapedAgentSchedules = DataShaper.ShapeData(pagedAgentSchedules, agentScheduleQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentSchedules, filteredAgentSchedules.Count(), agentScheduleQueryparameter.PageNumber, agentScheduleQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId));

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedule> GetAgentScheduleByEmployeeId(AgentAdminEmployeeIdDetails agentAdminEmployeeIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, agentAdminEmployeeIdDetails.Id);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedules by employee ids.
        /// </summary>
        /// <param name="employeeIds">The employee ids.</param>
        /// <returns></returns>
        public async Task<List<AgentSchedule>>GetAgentSchedulesByEmployeeIds(List<string> employeeIds)
        {
            var query =
                Builders<AgentSchedule>.Filter.Where(i => employeeIds.Contains(i.EmployeeId));

            var schedules = FilterBy(query);
            return await Task.FromResult(schedules.ToList());
        }

        /// <summary>
        /// Creates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        public void CreateAgentSchedule(AgentSchedule agentScheduleRequest)
        {
            InsertOneAsync(agentScheduleRequest);
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleRequest">The agent schedule request.</param>
        public void UpdateAgentSchedule(AgentSchedule agentScheduleRequest)
        {
            ReplaceOneAsync(agentScheduleRequest);
        }

        /// <summary>
        /// Bulks the update agent schedule.
        /// </summary>
        /// <param name="agentSchedule">The agent schedule.</param>
        /// <param name="copyAgentScheduleRequest">The copy agent schedule request.</param>
        public void BulkUpdateAgentScheduleCharts(AgentSchedule agentSchedule, CopyAgentSchedule copyAgentScheduleRequest)
        {
            var filter =
                Builders<AgentSchedule>.Filter.Where(i => copyAgentScheduleRequest.EmployeeIds.Contains(i.EmployeeId));

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.ModifiedBy, agentSchedule.ModifiedBy);

            switch (copyAgentScheduleRequest.AgentScheduleType)
            {
                case AgentScheduleType.SchedulingTab:
                    update =  update.Set(x => x.AgentScheduleCharts, agentSchedule.AgentScheduleCharts);
                    break;

                case AgentScheduleType.SchedulingMangerTab:
                    update = update.Set(x => x.AgentScheduleManagerCharts, agentSchedule.AgentScheduleManagerCharts);
                    break;

                default:
                    break;
            }

            UpdateManyAsync(filter, update);
        }

        /// <summary>
        /// Deletes the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        public void DeleteAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            DeleteByIdAsync(agentScheduleIdDetails.AgentScheduleId);
        }

        /// <summary>
        /// Filters the agent schedules.
        /// </summary>
        /// <param name="agentSchedules">The agent admins.</param>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        private IQueryable<AgentSchedule> FilterAgentSchedules(IQueryable<AgentSchedule> agentSchedules, AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            if (!agentSchedules.Any())
            {
                return agentSchedules;
            }

            if (agentScheduleQueryparameter.ClientId.HasValue && agentScheduleQueryparameter.ClientId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.ClientId == agentScheduleQueryparameter.ClientId);
            }

            if (agentScheduleQueryparameter.ClientLobGroupId.HasValue && agentScheduleQueryparameter.ClientLobGroupId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.ClientLobGroupId == agentScheduleQueryparameter.ClientLobGroupId);
            }

            if (agentScheduleQueryparameter.SkillGroupId.HasValue && agentScheduleQueryparameter.SkillGroupId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.SkillGroupId == agentScheduleQueryparameter.SkillGroupId);
            }

            if (agentScheduleQueryparameter.SkillTagId.HasValue && agentScheduleQueryparameter.SkillTagId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.SkillTagId == agentScheduleQueryparameter.SkillTagId);
            }

            if (!string.IsNullOrWhiteSpace(agentScheduleQueryparameter.SearchKeyword))
            {
                agentSchedules = agentSchedules.Where(o => string.Equals(o.ModifiedBy, agentScheduleQueryparameter.SearchKeyword, StringComparison.OrdinalIgnoreCase));
            }

            return agentSchedules;
        }
    }
}

