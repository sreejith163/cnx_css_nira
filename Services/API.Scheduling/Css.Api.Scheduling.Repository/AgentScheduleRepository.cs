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
        /// Gets the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<AgentScheduleRange> GetAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.AgentScheduleRanges, range => range.DateFrom == new DateTimeOffset(dateRange.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero) &&
                                                         range.DateTo == new DateTimeOffset(dateRange.DateTo.Date.ToUniversalTime(), TimeSpan.Zero)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var agentSchedule = await FindByIdAsync(query);
            return agentSchedule?.AgentScheduleRanges.FirstOrDefault(x => x.DateFrom == new DateTimeOffset(dateRange.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero) &&
                                                                          x.DateTo == new DateTimeOffset(dateRange.DateTo.Date.ToUniversalTime(), TimeSpan.Zero));
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier details].
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<bool> IsAgentScheduleRangeExist(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var dateFrom = new DateTimeOffset(dateRange.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero);
            var dateTo = new DateTimeOffset(dateRange.DateTo.Date.ToUniversalTime(), TimeSpan.Zero);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.AgentScheduleRanges, range => range.Status != SchedulingStatus.Rejected &&
                                                         dateRange.DateFrom <= range.DateTo && dateRange.DateTo >= range.DateFrom) &
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
                Builders<AgentSchedule>.Filter.ElemMatch(i => i.AgentScheduleRanges, range => range.AgentSchedulingGroupId == agentSchedulingGroupIdDetails.AgentSchedulingGroupId);

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
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.AgentScheduleRanges, range => range.DateFrom == new DateTimeOffset(agentScheduleDetails.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero) &&
                                                         range.DateTo == new DateTimeOffset(agentScheduleDetails.DateTo.Date.ToUniversalTime(), TimeSpan.Zero)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.AgentScheduleRanges[-1].Status, agentScheduleDetails.Status)
                .Set(x => x.ModifiedBy, agentScheduleDetails.ModifiedBy)
                .Set(x => x.ModifiedDate, DateTimeOffset.UtcNow);

            var documentCount = FindCountByIdAsync(query).Result;
            if (documentCount > 0)
            {
                update = update.Set(x => x.AgentScheduleRanges[-1].Status, agentScheduleDetails.Status);
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
                .Set(x => x.CurrentAgentShedulingGroupId, updateAgentScheduleEmployeeDetails.AgentSchedulingGroupId)
                .Set(x => x.AgentScheduleRanges[-1].AgentSchedulingGroupId, updateAgentScheduleEmployeeDetails.AgentSchedulingGroupId)
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
            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.AgentScheduleRanges, range => range.Status == SchedulingStatus.Pending_Schedule &&
                                                         range.DateFrom == new DateTimeOffset(agentScheduleRange.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero) &&
                                                         range.DateTo == new DateTimeOffset(agentScheduleRange.DateTo.Date.ToUniversalTime(), TimeSpan.Zero)) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var document = FindByIdAsync(query).Result;
            if (document != null)
            {
                var scheduleRange = document.AgentScheduleRanges.FirstOrDefault(
                    x => x.Status == SchedulingStatus.Pending_Schedule &&
                    x.DateFrom == new DateTimeOffset(agentScheduleRange.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero) &&
                    x.DateTo == new DateTimeOffset(agentScheduleRange.DateTo.Date.ToUniversalTime(), TimeSpan.Zero));

                if (scheduleRange.AgentScheduleCharts.Any())
                {
                    var update = Builders<AgentSchedule>.Update.Set(x => x.AgentScheduleRanges[-1], agentScheduleRange);
                    UpdateOneAsync(query, update);
                }
                else
                {
                    var update = Builders<AgentSchedule>.Update
                        .PullFilter(x => x.AgentScheduleRanges, builder => builder.Status == SchedulingStatus.Pending_Schedule &&
                                                                           builder.DateFrom == new DateTimeOffset(agentScheduleRange.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero) &&
                                                                           builder.DateTo == new DateTimeOffset(agentScheduleRange.DateTo.Date.ToUniversalTime(), TimeSpan.Zero));
                    UpdateOneAsync(query, update);
                }
            }
            else
            {
                var update = Builders<AgentSchedule>.Update.AddToSet(x => x.AgentScheduleRanges, agentScheduleRange);
                UpdateOneAsync(query, update);
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
                .AddToSet(x => x.AgentScheduleRanges, agentScheduleRange);

            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        public void UpdateAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRange)
        {
            var newDateFrom = new DateTimeOffset(dateRange.NewDateFrom.Date.ToUniversalTime(), TimeSpan.Zero);
            var newDateTo = new DateTimeOffset(dateRange.NewDateTo.Date.ToUniversalTime(), TimeSpan.Zero);
            var oldDateFrom = new DateTimeOffset(dateRange.OldDateFrom.Date.ToUniversalTime(), TimeSpan.Zero);
            var oldDateTo = new DateTimeOffset(dateRange.OldDateTo.Date.ToUniversalTime(), TimeSpan.Zero);

            var query =
                Builders<AgentSchedule>.Filter.Eq(i => i.Id, new ObjectId(agentScheduleIdDetails.AgentScheduleId)) &
                Builders<AgentSchedule>.Filter.ElemMatch(
                    i => i.AgentScheduleRanges, range => range.Status != SchedulingStatus.Pending_Schedule &&
                                                         oldDateFrom != range.DateFrom && oldDateTo != range.DateTo &&
                                                         dateRange.NewDateFrom <= range.DateTo && dateRange.NewDateTo >= range.DateFrom) &
                Builders<AgentSchedule>.Filter.Eq(i => i.IsDeleted, false);

            var update = Builders<AgentSchedule>.Update
                .Set(x => x.AgentScheduleRanges[-1].DateFrom, dateRange.NewDateFrom)
                .Set(x => x.AgentScheduleRanges[-1].DateTo, dateRange.NewDateTo)
                .Set(x => x.AgentScheduleRanges[-1].ModifiedBy, dateRange.ModifiedBy)
                .Set(x => x.AgentScheduleRanges[-1].ModifiedDate, DateTimeOffset.UtcNow);

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

            var update = Builders<AgentSchedule>.Update
                .PullFilter(x => x.AgentScheduleRanges, builder => builder.Status == SchedulingStatus.Pending_Schedule &&
                                                                   builder.DateFrom == new DateTimeOffset(dateRange.DateFrom.Date.ToUniversalTime(), TimeSpan.Zero) && 
                                                                   builder.DateTo == new DateTimeOffset(dateRange.DateTo.Date.ToUniversalTime(), TimeSpan.Zero));

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
                                                           (o.AgentScheduleRanges.Any(x => x.Status == scheduleStatus && !string.IsNullOrWhiteSpace(status))) ||
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
                agentSchedules = agentSchedules.Where(x => x.AgentScheduleRanges.Exists(y => y.AgentSchedulingGroupId == agentScheduleQueryparameter.AgentSchedulingGroupId));
            }

            if (agentScheduleQueryparameter.Status.HasValue)
            {
                agentSchedules = agentSchedules.Where(x => x.AgentScheduleRanges.Exists(y => y.Status == agentScheduleQueryparameter.Status));
            }

            if (agentScheduleQueryparameter.ExcludeConflictSchedule.HasValue && agentScheduleQueryparameter.DateFrom.HasValue && 
                agentScheduleQueryparameter.DateFrom != default(DateTimeOffset) && agentScheduleQueryparameter.DateTo.HasValue &&
                agentScheduleQueryparameter.DateTo != default(DateTimeOffset))
            {
                var dateFrom = new DateTimeOffset(agentScheduleQueryparameter.DateFrom.Value.Date.ToUniversalTime(), TimeSpan.Zero);
                var dateTo = new DateTimeOffset(agentScheduleQueryparameter.DateTo.Value.Date.ToUniversalTime(), TimeSpan.Zero);

                agentSchedules = agentSchedules.Where(x => x.AgentScheduleRanges.Exists(y => y.Status != SchedulingStatus.Approved &&
                                                                                             dateFrom <= y.DateTo && dateTo >= y.DateFrom));
            }

            return agentSchedules;
        }
    }
}

