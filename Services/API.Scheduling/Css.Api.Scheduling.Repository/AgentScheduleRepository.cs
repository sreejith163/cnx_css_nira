using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Css.Api.Core.Models.Enums;
using AutoMapper;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

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
        public AgentScheduleRepository(IMongoContext mongoContext,
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
            var agentSchedules = FilterBy(x => x.IsDeleted == false);

            var filteredAgentSchedules = FilterAgentSchedules(agentSchedules, agentScheduleQueryparameter);

            var sortedAgentSchedules = SortHelper.ApplySort(filteredAgentSchedules, agentScheduleQueryparameter.OrderBy);

            var pagedAgentSchedules = sortedAgentSchedules;

            if (!agentScheduleQueryparameter.SkipPageSize)
            {
                pagedAgentSchedules = pagedAgentSchedules
                    .Skip((agentScheduleQueryparameter.PageNumber - 1) * agentScheduleQueryparameter.PageSize)
                    .Take(agentScheduleQueryparameter.PageSize);
            }

            var mappedAgentAdmins = pagedAgentSchedules
                .ProjectTo<AgentScheduleDTO>(_mapper.ConfigurationProvider);

            var shapedAgentSchedules = DataShaper.ShapeData(mappedAgentAdmins, agentScheduleQueryparameter.Fields);

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
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange, SchedulingStatus status)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.Ranges, range => range.Status == status && range.DateFrom == dateRange.DateFrom && range.DateTo == dateRange.DateTo) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<AgentSchedule> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.Ranges, range => range.DateFrom == dateRange.DateFrom && range.DateTo == dateRange.DateTo) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<AgentScheduleRange> GetAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.Ranges, range => range.DateFrom == dateRange.DateFrom && range.DateTo == dateRange.DateTo) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var agentSchedule = await FindByIdAsync(query);
            return agentSchedule?.Ranges.FirstOrDefault(x => x.DateFrom == dateRange.DateFrom && x.DateTo == dateRange.DateTo);
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier details].
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<bool> IsAgentScheduleRangeExist(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.Ranges, range => range.Status != SchedulingStatus.Rejected &&
                                            ((dateRange.DateFrom < range.DateTo && dateRange.DateTo > range.DateFrom) ||
                                            (dateRange.DateFrom == range.DateFrom && dateRange.DateTo == range.DateTo))) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var count = await FindCountByIdAsync(query);
            return count > 0;
        }

        /// <summary>
        /// Gets the agent schedule count.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public async Task<long> GetAgentScheduleCount(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindCountByIdAsync(query);
        }

        /// <summary>
        /// Gets the employee identifier by agent schedule identifier.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public async Task<int> GetEmployeeIdByAgentScheduleId(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var agentSchedule = await FindByIdAsync(query);
            return agentSchedule.EmployeeId;
        }

        /// <summary>
        /// Gets the employee ids by agent schedule group identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetEmployeeIdsByAgentScheduleGroupId(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<AgentSchedule>.Filter.Eq(i => i.ActiveAgentSchedulingGroupId, agentSchedulingGroupIdDetails.AgentSchedulingGroupId);

            var agentSchedules = FilterBy(query);

            return await Task.FromResult(agentSchedules.Select(x => x.EmployeeId).ToList());
        }

        /// <summary>
        /// Gets the agent schedule by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedule> GetAgentScheduleByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, agentAdminEmployeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent schedule count by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        public async Task<long> GetAgentScheduleCountByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            return await FindCountByIdAsync(query);
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
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        public void UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month, 
                                                         agentScheduleDetails.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month, 
                                                       agentScheduleDetails.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.Ranges, range => range.DateFrom == agentScheduleDetails.DateFrom && range.DateTo == agentScheduleDetails.DateTo) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.Ranges[-1].Status, agentScheduleDetails.Status)
                .Set(x => x.ModifiedBy, agentScheduleDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            var documentCount = FindCountByIdAsync(query).Result;
            if (documentCount > 0)
            {
                update = update.Set(x => x.Ranges[-1].Status, agentScheduleDetails.Status);
            }

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="updateAgentScheduleEmployeeDetails">The update agent schedule employee details.</param>
        public void UpdateAgentSchedule(EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.EmployeeId, updateAgentScheduleEmployeeDetails.EmployeeId)
                .Set(x => x.FirstName, updateAgentScheduleEmployeeDetails.FirstName)
                .Set(x => x.LastName, updateAgentScheduleEmployeeDetails.LastName)
                .Set(x => x.ActiveAgentSchedulingGroupId, updateAgentScheduleEmployeeDetails.AgentSchedulingGroupId)
                .Set(x => x.ModifiedBy, updateAgentScheduleEmployeeDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        /// <param name="modifiedUserDetails">The modified user details.</param>
        public void UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleRange agentScheduleRange, ModifiedUserDetails modifiedUserDetails)
        {
            agentScheduleRange.DateFrom = new DateTime(agentScheduleRange.DateFrom.Year, agentScheduleRange.DateFrom.Month, 
                                                       agentScheduleRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleRange.DateTo = new DateTime(agentScheduleRange.DateTo.Year, agentScheduleRange.DateTo.Month, 
                                                     agentScheduleRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var scheduleRangeQuery = query & Builders<AgentSchedule>.Filter
                .ElemMatch(i => i.Ranges, range => range.Status == SchedulingStatus.Pending_Schedule &&
                                                   range.DateFrom == agentScheduleRange.DateFrom &&
                                                   range.DateTo == agentScheduleRange.DateTo);

            var document = FindByIdAsync(query).Result;
            if (document != null)
            {
                var scheduleRange = document.Ranges.FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                                        x.DateFrom == agentScheduleRange.DateFrom &&
                                                                        x.DateTo == agentScheduleRange.DateTo);

                if (scheduleRange != null)
                {
                    query = scheduleRangeQuery;
                    if (agentScheduleRange.ScheduleCharts.Any())
                    {
                        var update = Builders<AgentSchedule>.Update
                            .Set(x => x.Ranges[-1], agentScheduleRange);
                        UpdateOneAsync(query, update);
                    }
                    else
                    {
                        var update = Builders<AgentSchedule>.Update
                            .PullFilter(x => x.Ranges, builder => builder.Status == SchedulingStatus.Pending_Schedule &&
                                                                  builder.DateFrom == agentScheduleRange.DateFrom &&
                                                                  builder.DateTo == agentScheduleRange.DateTo);
                        UpdateOneAsync(query, update);
                    }
                }
                else
                {
                    var update = Builders<AgentSchedule>.Update.AddToSet(x => x.Ranges, agentScheduleRange);
                    UpdateOneAsync(query, update);
                }
            }
        }

        /// <summary>
        /// Copies the agent schedules.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        public void CopyAgentSchedules(EmployeeIdDetails employeeIdDetails, AgentScheduleRange agentScheduleRange)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .AddToSet(x => x.Ranges, agentScheduleRange);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        public void UpdateAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRange)
        {
            var newDateFrom = new DateTime(dateRange.NewDateFrom.Year, dateRange.NewDateFrom.Month, dateRange.NewDateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            var newDateTo = new DateTime(dateRange.NewDateTo.Year, dateRange.NewDateTo.Month, dateRange.NewDateTo.Day, 0, 0, 0, DateTimeKind.Utc);
            var oldDateFrom = new DateTime(dateRange.OldDateFrom.Year, dateRange.OldDateFrom.Month, dateRange.OldDateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            var oldDateTo = new DateTime(dateRange.OldDateTo.Year, dateRange.OldDateTo.Month, dateRange.OldDateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.Ranges, range => range.Status == SchedulingStatus.Pending_Schedule &&
                                            range.DateFrom == oldDateFrom && range.DateTo == oldDateTo) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);


            var update = Builders<AgentSchedule>.Update
                .Set(x => x.Ranges[-1].ModifiedBy, dateRange.ModifiedBy)
                .Set(x => x.Ranges[-1].ModifiedDate, DateTimeOffset.UtcNow)
                .Set(x => x.Ranges[-1].DateFrom, newDateFrom)
                .Set(x => x.Ranges[-1].DateTo, newDateTo);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Deletes the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        public void DeleteAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            dateRange.DateFrom = new DateTime(dateRange.DateFrom.Year, dateRange.DateFrom.Month, dateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            dateRange.DateTo = new DateTime(dateRange.DateTo.Year, dateRange.DateTo.Month, dateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var update = Builders<AgentSchedule>.Update
                .PullFilter(x => x.Ranges, builder => builder.Status == SchedulingStatus.Pending_Schedule &&
                                                      builder.DateFrom == dateRange.DateFrom &&
                                                      builder.DateTo == dateRange.DateTo);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Deletes the agent schedule.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        public void DeleteAgentSchedule(EmployeeIdDetails employeeIdDetails)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.IsDeleted, true)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            UpdateOneAsync(query, update);
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

            if (!string.IsNullOrWhiteSpace(agentScheduleQueryparameter.SearchKeyword))
            {
                SchedulingStatus scheduleStatus = SchedulingStatus.Approved;
                int.TryParse(agentScheduleQueryparameter.SearchKeyword.Trim(), out int employeeId);
                var status = Enum.GetNames(typeof(SchedulingStatus)).FirstOrDefault(e => e.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.ToLower()));

                if (!string.IsNullOrWhiteSpace(status))
                {
                    scheduleStatus = (SchedulingStatus)Enum.Parse(typeof(SchedulingStatus), status, true);
                }

                agentSchedules = agentSchedules.Where(o => (o.EmployeeId == employeeId && employeeId != 0) ||
                                                           (o.Ranges.Any(x => x.Status == scheduleStatus && !string.IsNullOrWhiteSpace(status))) ||
                                                            o.FirstName.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()) ||
                                                            o.LastName.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()) ||
                                                            o.CreatedBy.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()) ||
                                                            o.ModifiedBy.ToLower().Contains(agentScheduleQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            if (agentScheduleQueryparameter.EmployeeIds.Any())
            {
                agentSchedules = agentSchedules.Where(x => agentScheduleQueryparameter.EmployeeIds.Distinct().ToList().Contains(x.EmployeeId));
            }

            if (agentScheduleQueryparameter.AgentSchedulingGroupId.HasValue && agentScheduleQueryparameter.AgentSchedulingGroupId != default(int))
            {
                agentSchedules = agentSchedules.Where(x => x.ActiveAgentSchedulingGroupId == agentScheduleQueryparameter.AgentSchedulingGroupId);
            }

            if (agentScheduleQueryparameter.Status.HasValue)
            {
                agentSchedules = agentSchedules.Where(x => x.Ranges.Exists(y => y.Status == agentScheduleQueryparameter.Status));
            }

            if (agentScheduleQueryparameter.DateFrom.HasValue && agentScheduleQueryparameter.DateFrom != default(DateTime) && 
                agentScheduleQueryparameter.DateTo.HasValue && agentScheduleQueryparameter.DateTo != default(DateTime) &&
                !agentScheduleQueryparameter.ExcludeConflictSchedule)
            {
                agentScheduleQueryparameter.DateFrom = new DateTime(agentScheduleQueryparameter.DateFrom.Value.Year, agentScheduleQueryparameter.DateFrom.Value.Month,
                                                                    agentScheduleQueryparameter.DateFrom.Value.Day, 0, 0, 0, DateTimeKind.Utc);
                agentScheduleQueryparameter.DateTo = new DateTime(agentScheduleQueryparameter.DateTo.Value.Year, agentScheduleQueryparameter.DateTo.Value.Month,
                                                                    agentScheduleQueryparameter.DateTo.Value.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedules = agentSchedules.Where(x => x.Ranges.Any(y => agentScheduleQueryparameter.DateFrom == y.DateFrom &&
                                                                             agentScheduleQueryparameter.DateTo == y.DateTo));
            }

            if (agentScheduleQueryparameter.DateFrom.HasValue && agentScheduleQueryparameter.DateFrom != default(DateTime) &&
                agentScheduleQueryparameter.DateTo.HasValue && agentScheduleQueryparameter.DateTo != default(DateTime) &&
                agentScheduleQueryparameter.ExcludeConflictSchedule)
            {
                agentScheduleQueryparameter.DateFrom = new DateTime(agentScheduleQueryparameter.DateFrom.Value.Year, agentScheduleQueryparameter.DateFrom.Value.Month,
                                                                    agentScheduleQueryparameter.DateFrom.Value.Day, 0, 0, 0, DateTimeKind.Utc);
                agentScheduleQueryparameter.DateTo = new DateTime(agentScheduleQueryparameter.DateTo.Value.Year, agentScheduleQueryparameter.DateTo.Value.Month,
                                                                    agentScheduleQueryparameter.DateTo.Value.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedules = agentSchedules.Where(x => !x.Ranges.Any(y => ((agentScheduleQueryparameter.DateFrom < y.DateTo &&
                                                                               agentScheduleQueryparameter.DateTo > y.DateFrom) ||
                                                                              (agentScheduleQueryparameter.DateFrom == y.DateFrom &&
                                                                               agentScheduleQueryparameter.DateTo == y.DateTo))));
            }

            return agentSchedules;
        }
    }
}

