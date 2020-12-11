using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.ClientName;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>
    /// Service for agent admin part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.IAgentAdminService" />
    public class AgentAdminService : IAgentAdminService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IAgentAdminRepository _repository;

        /// <summary>The skill tag repository</summary>
        private readonly ISkillTagRepository _skillTagRepository;

        /// <summary>The client name repository</summary>
        private readonly IClientNameRepository _clientNameRepository;

        /// <summary>The client lob group repository</summary>
        private readonly IClientLobGroupRepository _clientLobGroupRepository;

        /// <summary>The skill group repository</summary>
        private readonly ISkillGroupRepository _skillGroupRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;


        /// <summary>Initializes a new instance of the <see cref="AgentAdminService" /> class.</summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentAdminService(
            IHttpContextAccessor httpContextAccessor,
            IAgentAdminRepository repository,
            IClientNameRepository clientNameRepository,
            IClientLobGroupRepository clientLobGroupRepository,
            ISkillGroupRepository skillGroupRepository,
            ISkillTagRepository skillTagRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _clientNameRepository = clientNameRepository;
            _clientLobGroupRepository = clientLobGroupRepository;
            _skillGroupRepository = skillGroupRepository;
            _skillTagRepository = skillTagRepository;
            _mapper = mapper;
            _uow = uow;
            SeedData();
        }

        //To be changed
        public async void SeedData()
        {
            var isClientNameSeeded = await _clientNameRepository.GetClientNamesCount() > 0;
            if (!isClientNameSeeded)
            {
                _clientNameRepository.CreateClientNames(new List<ClientCollection>
                {
                    new ClientCollection{ClientId=1, Name="Client Name 1"},
                    new ClientCollection{ClientId=2, Name="Client Name 2"}

                });
            }

            var isClientLobSeeded = await _clientLobGroupRepository.GetClientLobGroupsCount() > 0;
            if (!isClientLobSeeded)
            {
                _clientLobGroupRepository.CreateClientLobGroups(new List<ClientLobCollection>
                {
                    new ClientLobCollection{ClientId=1, ClientLobGroupId=1, Name="Client Lob 1"},
                    new ClientLobCollection{ClientId=2, ClientLobGroupId=2, Name="Client Lob 2"}

                });
            }

            var isSkillGroupSeeded = await _skillGroupRepository.GetSkillGroupsCount() > 0;
            if (!isSkillGroupSeeded)
            {
                _skillGroupRepository.CreateSkillGroups(new List<SkillGroupCollection>
                {
                    new SkillGroupCollection{ClientId=1, ClientLobGroupId=1, SkillGroupId=1, Name="Skill Group 1"},
                    new SkillGroupCollection{ClientId=2, ClientLobGroupId=2, SkillGroupId=2, Name="Skill Group 2"}

                });
            }

            var isSkillTagSeeded = await _skillTagRepository.GetSkillTagsCount() > 0;
            if (!isSkillTagSeeded)
            {
                _skillTagRepository.CreateSkillTags(new List<SkillTagCollection>
                {
                    new SkillTagCollection{ClientId=1, ClientLobGroupId=1, SkillGroupId=1, SkillTagId=1, Name="Skill Tag 1"},
                    new SkillTagCollection{ClientId=2, ClientLobGroupId=2, SkillGroupId=2, SkillTagId=2, Name="Skill Tag 2"}
                });
            }

            await _uow.Commit();
        }

        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter)
        {
            var agentAdmins = await _repository.GetAgentAdmins(agentAdminQueryParameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentAdmins));

            return new CSSResponse(agentAdmins, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var agentAdmin = await _repository.GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillTag = await _skillTagRepository.GetSkillTag(new SkillTagIdDetails
            {
                SkillTagId = agentAdmin.SkillTagId
            });
            if (skillTag == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillGroup = await _skillGroupRepository.GetSkillGroup(new SkillGroupIdDetails
            {
                SkillGroupId = skillTag.SkillGroupId
            });
            if (skillGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientLobGroup = await _clientLobGroupRepository.GetClientLobGroup(new ClientLobGroupIdDetails
            {
                ClientLobGroupId = skillGroup.ClientLobGroupId
            });
            if (clientLobGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientName = await _clientNameRepository.GetClientName(new ClientNameIdDetails
            {
                ClientNameId = clientLobGroup.ClientId
            });
            if (clientName == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentAdmin = _mapper.Map<AgentAdminDetailsDTO>(agentAdmin);
            mappedAgentAdmin.SkillTagName = skillTag.Name;
            mappedAgentAdmin.SkillGroupId = skillGroup.SkillGroupId;
            mappedAgentAdmin.SkillGroupName = skillGroup.Name;
            mappedAgentAdmin.ClientLobGroupId = clientLobGroup.ClientLobGroupId;
            mappedAgentAdmin.ClientLobGroupName = clientLobGroup.Name;
            mappedAgentAdmin.ClientId = clientName.ClientId;
            mappedAgentAdmin.ClientName = clientName.Name;

            return new CSSResponse(mappedAgentAdmin, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="agentAdminDetails">The agent admin details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateAgentAdmin(CreateAgentAdmin agentAdminDetails)
        {
            //To be changed
            //var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentAdminDetails.SkillTagId };

            //var skillTag = await _repository.GetSkillTag(skillTagIdDetails);
            //if (skillTag == null)
            //{
            //    return new CSSResponse($"Skill Tag with id '{skillTagIdDetails.SkillTagId}' not found", HttpStatusCode.NotFound);
            //}

            var agentAdminEmployeeIdDetails = new AgentAdminEmployeeIdDetails { Id = agentAdminDetails.EmployeeId };

            var agentAdmins = await _repository.GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdmins != null)
            {
                return new CSSResponse($"Agent Admin with Employee id '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };

            var agentAdminIds = await _repository.GetAgentAdminIdsBySso(agentAdminSsoDetails);

            if (agentAdminIds != null)
            {
                return new CSSResponse($"Agent Admin with Sso '{agentAdminSsoDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminRequest = _mapper.Map<AgentCollection>(agentAdminDetails);

            var agentAdminCount = await _repository.GetAgentAdminsCount();

            agentAdminRequest.AgentAdminId = agentAdminCount + 1;

            _repository.CreateAgentAdmin(agentAdminRequest);

            await _uow.Commit();

            return new CSSResponse(new AgentAdminIdDetails { AgentAdminId = agentAdminRequest.AgentAdminId }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <param name="agentAdminDetails">The agent admin details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentAdmin(AgentAdminIdDetails agentAdminIdDetails, UpdateAgentAdmin agentAdminDetails)
        {
            var agentAdmin = await _repository.GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentAdminEmployeeIdDetails = new AgentAdminEmployeeIdDetails { Id = agentAdminDetails.EmployeeId };

            var agentAdmins = await _repository.GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdmins != null && agentAdmins.AgentAdminId != agentAdminIdDetails.AgentAdminId)
            {
                return new CSSResponse($"Agent Admin with Employee id '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };

            var agentAdminIds = await _repository.GetAgentAdminIdsBySso(agentAdminSsoDetails);

            if (agentAdminIds != null && agentAdminIds.AgentAdminId != agentAdminIdDetails.AgentAdminId)
            {
                return new CSSResponse($"Agent Admin with Sso '{agentAdminSsoDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminRequest = _mapper.Map(agentAdminDetails, agentAdmin);

            _repository.UpdateAgentAdmin(agentAdminRequest);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var agentAdmin = await _repository.GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            agentAdmin.IsDeleted = true;

            _repository.UpdateAgentAdmin(agentAdmin);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}