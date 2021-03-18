using AutoMapper;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Response.EntityHierarchy;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>
    /// Service for entity hierarchy part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.IEntityHierarchyService" />
    public class EntityHierarchyService : IEntityHierarchyService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The client name repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The client lob group repository
        /// </summary>
        private readonly IClientLobGroupRepository _clientLobGroupRepository;

        /// <summary>
        /// The skill group repository
        /// </summary>
        private readonly ISkillGroupRepository _skillGroupRepository;

        /// <summary>
        /// The skill tag repository
        /// </summary>
        private readonly ISkillTagRepository _skillTagRepository;

        /// <summary>
        /// The agent scheduling group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>Initializes a new instance of the <see cref="EntityHierarchyService" /> class.</summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="clientLobGroupRepository">The client lob group repository.</param>
        /// <param name="skillGroupRepository">The skill group repository.</param>
        /// <param name="skillTagRepository">The skill tag repository.</param>
        /// <param name="agentSchedulingGroupRepository">The agent scheduling group repository.</param>
        /// <param name="mapper">The mapper.</param>
        public EntityHierarchyService(
            IHttpContextAccessor httpContextAccessor,

            IClientRepository clientRepository,
            IClientLobGroupRepository clientLobGroupRepository,
            ISkillGroupRepository skillGroupRepository,
            ISkillTagRepository skillTagRepository,
            IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            IMapper mapper
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _clientRepository = clientRepository;
            _clientLobGroupRepository = clientLobGroupRepository;
            _skillGroupRepository = skillGroupRepository;
            _skillTagRepository = skillTagRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _mapper = mapper;
        }


        /// <summary>Gets the entity hierarchy.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetEntityHierarchy(ClientIdDetails clientIdDetails)
        {
            EntityHierarchyDTO entityHierarchyDTO;
            var clientDetails = await _clientRepository.GetClient(new ClientIdDetails
            {
                ClientId = clientIdDetails.ClientId
            });

            if (clientDetails == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            entityHierarchyDTO = new EntityHierarchyDTO();
            entityHierarchyDTO.Client = _mapper.Map<ClientDTO>(clientDetails);

            var clientLobGroupsOfClient =
                await _clientLobGroupRepository.GetClientLobGroupsOfClient(clientIdDetails);

            if (clientLobGroupsOfClient != null &&
                    clientLobGroupsOfClient.ToList() != null && clientLobGroupsOfClient.Count() > 0)
            {
                entityHierarchyDTO.Client.ClientLOBs =
                    _mapper.Map<List<ClientLobDTO>>(clientLobGroupsOfClient.ToList());

                foreach (var clientLobGroup in clientLobGroupsOfClient)
                {
                    var skillGroupsOfClientLOB =
                        await _skillGroupRepository.GetSkillGroupsOfClientLOB(new ClientLobGroupIdDetails
                        {
                            ClientLobGroupId = clientLobGroup.ClientLobGroupId
                        });

                    if (skillGroupsOfClientLOB != null &&
                            skillGroupsOfClientLOB.ToList() != null && skillGroupsOfClientLOB.Count() > 0)
                    {
                        entityHierarchyDTO.Client.ClientLOBs
                            .Where(x => x.Id.Equals(clientLobGroup.ClientLobGroupId.ToString())).FirstOrDefault()
                            .SkillGroups = _mapper.Map<List<SkillGroupDTO>>(skillGroupsOfClientLOB.ToList());

                        foreach (var skillGroup in skillGroupsOfClientLOB)
                        {
                            var skillTagsOfSkillGroup = await _skillTagRepository.GetSkillTagsOfSkillGroup(new SkillGroupIdDetails
                            {
                                SkillGroupId = skillGroup.SkillGroupId
                            });

                            if (skillTagsOfSkillGroup != null &&
                                skillTagsOfSkillGroup.ToList() != null && skillTagsOfSkillGroup.Count() > 0)
                            {
                                entityHierarchyDTO.Client.ClientLOBs
                                 .Where(x => x.Id.Equals(clientLobGroup.ClientLobGroupId.ToString())).FirstOrDefault()
                                 .SkillGroups.Where(x => x.Id.Equals(skillGroup.SkillGroupId.ToString())).FirstOrDefault()
                                 .SkillTags = _mapper.Map<List<SkillTagDTO>>(skillTagsOfSkillGroup.ToList());

                                foreach (var skillTag in skillTagsOfSkillGroup)
                                {
                                    var agentSchedulingGroupsOfSkillTag =
                                        await _agentSchedulingGroupRepository.GetAgentSchedulingGroupBySkillTag(new SkillTagIdDetails
                                        {
                                            SkillTagId = skillTag.SkillTagId
                                        });

                                    foreach (var agentSchedulingGroup in agentSchedulingGroupsOfSkillTag)
                                    {
                                        if (entityHierarchyDTO.AgentSchedulingGroups == null)
                                        {
                                            entityHierarchyDTO.AgentSchedulingGroups = new List<AgentSchedulingGroupDTO>();
                                        }

                                        var agentSchedulingGroupDTO = _mapper.Map<AgentSchedulingGroupDTO>(agentSchedulingGroup);
                                        agentSchedulingGroupDTO.ClientId = clientDetails.ClientId;
                                        agentSchedulingGroupDTO.ClientRefId = clientDetails.RefId;
                                        agentSchedulingGroupDTO.ClientName = clientDetails.Name;
                                        agentSchedulingGroupDTO.ClientLOBId = clientLobGroup.ClientLobGroupId;
                                        agentSchedulingGroupDTO.ClientLOBRefId = clientLobGroup.RefId;
                                        agentSchedulingGroupDTO.ClientLOBName = clientLobGroup.Name;
                                        agentSchedulingGroupDTO.SkillGroupId = skillGroup.SkillGroupId;
                                        agentSchedulingGroupDTO.SkillGroupRefId = skillGroup.RefId;
                                        agentSchedulingGroupDTO.SkillGroupName = skillGroup.Name;
                                        agentSchedulingGroupDTO.SkillTagId = skillTag.SkillTagId;
                                        agentSchedulingGroupDTO.SkillTagRefId = skillTag.RefId;
                                        agentSchedulingGroupDTO.SkillTagName = skillTag.Name;


                                        entityHierarchyDTO.AgentSchedulingGroups.Add(agentSchedulingGroupDTO);
                                    }

                                }
                            }
                        }

                    }
                }
            }

            return new CSSResponse(entityHierarchyDTO, HttpStatusCode.OK);
        }
    }
}