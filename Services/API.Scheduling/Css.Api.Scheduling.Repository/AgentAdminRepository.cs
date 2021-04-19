using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentAdminRepository : GenericRepository<Agent>, IAgentAdminRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>Initializes a new instance of the <see cref="AgentAdminRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentAdminRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter)
        {
            var agentAdmins = FilterBy(x => x.IsDeleted == false);

            var filteredAgentAdmins = FilterAgentAdmin(agentAdmins, agentAdminQueryParameter.SearchKeyword, agentAdminQueryParameter.AgentSchedulingGroupId);

            var sortedAgentAdmins = SortHelper.ApplySort(filteredAgentAdmins, agentAdminQueryParameter.OrderBy);

            var pagedAgentAdmins = sortedAgentAdmins;

            if (!agentAdminQueryParameter.SkipPageSize)
            {
                pagedAgentAdmins = sortedAgentAdmins
                   .Skip((agentAdminQueryParameter.PageNumber - 1) * agentAdminQueryParameter.PageSize)
                   .Take(agentAdminQueryParameter.PageSize);
            }

            var mappedAgentAdmins = pagedAgentAdmins
            .ProjectTo<AgentAdminDTO>(_mapper.ConfigurationProvider);

            var shapedAgentAdmins = DataShaper.ShapeData(mappedAgentAdmins, agentAdminQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentAdmins, filteredAgentAdmins.Count(), agentAdminQueryParameter.PageNumber, agentAdminQueryParameter.PageSize);
        }

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public async Task<Agent> GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.Eq(i => i.Id, new ObjectId(agentAdminIdDetails.AgentAdminId));

            return await FindByIdAsync(query);
        }

        //To be changed
        /// <summary>
        /// Gets the agent admins by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <param name="agentAdminSsoDetails">The agent admin sso details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Agent> GetAgentAdminIdsByEmployeeIdAndSso(EmployeeIdDetails agentAdminEmployeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.Eq(i => i.Ssn, agentAdminEmployeeIdDetails.Id) &
                Builders<Agent>.Filter.Eq(i => i.Sso, agentAdminSsoDetails.Sso);

            return await FindByIdAsync(query);
        }

        /// <summary>Gets the agent admin by sso.</summary>
        /// <param name="agentAdminSsoDetails">The agent admin sso details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Agent> GetAgentAdminBySso(AgentAdminSsoDetails agentAdminSsoDetails)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.Eq(i => i.Sso, agentAdminSsoDetails.Sso);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent admin by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        public async Task<Agent> GetAgentAdminByEmployeeId(EmployeeIdDetails agentAdminEmployeeIdDetails)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.Eq(i => i.Ssn, agentAdminEmployeeIdDetails.Id);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent admins by employee ids.
        /// </summary>
        /// <param name="agentAdminEmployeeIdsDetails">The agent admin employee ids details.</param>
        /// <returns></returns>
        public async Task<List<Agent>> GetAgentAdminsByEmployeeIds(List<string> agentAdminEmployeeIdsDetails)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.In(i => i.Ssn, agentAdminEmployeeIdsDetails);

            var agentAdmins = FilterBy(query);

            return await Task.FromResult(agentAdmins.ToList());
        }

        /// <summary>
        /// Gets the agent admins by category identifier.
        /// </summary>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        public async Task<List<Agent>> GetAgentAdminsByCategoryId(List<int> agentCategoryDetails)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.ElemMatch(
                    i => i.AgentCategoryValues, category => agentCategoryDetails.Contains(category.CategoryId));

            var agentAdmins = FilterBy(query);

            return await Task.FromResult(agentAdmins.ToList());
        }

        /// <summary>Gets the agent admins by ids.</summary>
        /// <param name="agentAdminIdsDetails">The agent admin ids details.</param>
        /// <param name="sourceSchedulingGroupId">The source scheduling group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<Agent>> GetAgentAdminsByIds(List<ObjectId> agentAdminIdsDetails, int sourceSchedulingGroupId)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.In(i => i.Id, agentAdminIdsDetails) &
                Builders<Agent>.Filter.Eq(i => i.AgentSchedulingGroupId, sourceSchedulingGroupId);

            var agentAdmins = FilterBy(query);

            return await Task.FromResult(agentAdmins.ToList());
        }

        /// <summary>
        /// Gets the employee ids by agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<List<string>> GetEmployeeIdsByAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.Eq(i => i.AgentSchedulingGroupId, agentSchedulingGroupIdDetails.AgentSchedulingGroupId);

            var employees = FilterBy(query);

            return await Task.FromResult(employees?.Select(x => x.Ssn).ToList());
        }


        public async Task<List<object>> GetAgentList()
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) ;

            var msg = new List<object>();
            var employees = FilterBy(query);
            foreach (var item in employees)
            {
                var x = new AgentList
                {
                    EmployeeId = item.Ssn

                };
                msg.Add(x);

            }





            return await Task.FromResult(msg);
        }

        /// <summary>
        /// Gets the agent admins count.
        /// </summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<int> GetAgentAdminsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Updates the agent category value.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="categoryValue">The category value.</param>
        public void UpdateAgentCategoryValue(EmployeeIdDetails employeeIdDetails, AgentCategoryValue categoryValue)
        {            
            var query =
                Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) & 
                Builders<Agent>.Filter.ElemMatch(
                    i => i.AgentCategoryValues, category => category.CategoryId == categoryValue.CategoryId) &
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);

            var documentCount = FindCountByIdAsync(query).Result;
            if (documentCount > 0)
            {
                var update = Builders<Agent>.Update
                    .Set(x => x.AgentCategoryValues[-1].StartDate, categoryValue.StartDate)
                    .Set(x => x.AgentCategoryValues[-1].CategoryValue, categoryValue.CategoryValue);

                UpdateOneAsync(query, update);
            }
            else
            {
                var update = Builders<Agent>.Update.AddToSet(x => x.AgentCategoryValues, categoryValue);
                UpdateOneAsync(query, update);
            }
        }
        /// <summary>
        /// Updates the agent category value.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="categoryValue">The category value.</param>
        public void ImportUpdateAgentCategoryValue(EmployeeIdDetails employeeIdDetails, AgentCategoryValue categoryValue)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) &
                Builders<Agent>.Filter.ElemMatch(
                    i => i.AgentCategoryValues, category => category.CategoryId == categoryValue.CategoryId) &
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);

            var documentCount = FindCountByIdAsync(query).Result;
            if (documentCount > 0)
            {
                var update = Builders<Agent>.Update
                    .Set(x => x.AgentCategoryValues[-1].StartDate, categoryValue.StartDate)
                    .Set(x => x.AgentCategoryValues[-1].CategoryValue, categoryValue.CategoryValue);

                UpdateOneAsync(query, update);
            }
            else
            {
               var query1 =
               Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) &
               Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);
                var update = Builders<Agent>.Update.Push(x => x.AgentCategoryValues, categoryValue);
                UpdateOneAsync(query1, update);
            }
        }
        public void UpdateHireDate(EmployeeIdDetails employeeIdDetails, DateTime hireDate)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) &
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);
            var update = Builders<Agent>.Update
              .Set(x => x.HireDate, hireDate);
             
            UpdateOneAsync(query, update);

        }
        public void UpdateFirstName(EmployeeIdDetails employeeIdDetails, string firstname)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) &
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);
            var update = Builders<Agent>.Update
              .Set(x => x.FirstName, firstname);

            UpdateOneAsync(query, update);

        }
        public void UpdateLastName(EmployeeIdDetails employeeIdDetails, string lastname)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) &
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);
            var update = Builders<Agent>.Update
              .Set(x => x.FirstName, lastname);

            UpdateOneAsync(query, update);

        }

        public void UpdateTeamLead(EmployeeIdDetails employeeIdDetails, string teamlead)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) &
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);
            var update = Builders<Agent>.Update
              .Set(x => x.SupervisorName, teamlead);

            UpdateOneAsync(query, update);

        }
        public void ImportUpdateAgent(EmployeeIdDetails employeeIdDetails, AgentSchedulingUpdateRequest agentSchedulingUpdateRequest)
        {
            var query =
                Builders<Agent>.Filter.Eq(i => i.Ssn, employeeIdDetails.Id) &
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false);
            var update = Builders<Agent>.Update
                .Set(x => x.Mu, agentSchedulingUpdateRequest.RefId.ToString())
                .Set(x => x.ClientId, agentSchedulingUpdateRequest.ClientId)
                .Set(x => x.ClientLobGroupId, agentSchedulingUpdateRequest.ClientLobGroupId)
                .Set(x => x.SkillGroupId, agentSchedulingUpdateRequest.SkillGroupId)
                .Set(x => x.SkillTagId, agentSchedulingUpdateRequest.SkillTagId)
                .Set(x => x.AgentSchedulingGroupId, agentSchedulingUpdateRequest.AgentSchedulingGroupId);
            UpdateOneAsync(query, update);
        }

        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void CreateAgentAdmin(Agent agentAdminRequest)
        {
            InsertOneAsync(agentAdminRequest);
        }

        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void UpdateAgentAdmin(Agent agentAdminRequest)
        {
            ReplaceOneAsync(agentAdminRequest);
        }

        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void DeleteAgentAdmin(Agent agentAdminRequest)
        {
            DeleteOneAsync(x => x.Id == agentAdminRequest.Id);
        }


        /// <summary>Filters the agent admin.</summary>
        /// <param name="agentAdmins">The agent admins.</param>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<Agent> FilterAgentAdmin(IQueryable<Agent> agentAdmins, string searchKeyword, int? agentSchedulingGroupId)
        {
            if (!agentAdmins.Any())
            {
                return agentAdmins;
            }

            if (agentSchedulingGroupId != null && agentSchedulingGroupId != default(int))
            {
                agentAdmins = agentAdmins.Where(x => x.AgentSchedulingGroupId == agentSchedulingGroupId);
            }

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                agentAdmins = agentAdmins.ToList().Where(o => o.Sso.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                                    o.FirstName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                                    o.LastName.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) ||
                                                    o.Ssn.ToString().Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)
                                                    ).AsQueryable();
            }

            return agentAdmins;
        }
    }
}

