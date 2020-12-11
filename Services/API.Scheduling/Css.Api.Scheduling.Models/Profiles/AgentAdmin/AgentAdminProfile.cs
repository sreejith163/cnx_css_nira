using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using System;

namespace Css.Api.Scheduling.Models.Profiles.AgentAdmin
{
    public class AgentAdminProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentAdminProfile" /> class.</summary>
        public AgentAdminProfile()
        {
            CreateMap<CreateAgentAdmin, Domain.Agent>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(o => o.FirstName.Trim()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(o => o.LastName.Trim()))
                .ForMember(x => x.Sso, opt => opt.MapFrom(o => o.Sso.Trim()))
                .ForMember(x => x.Ssn, opt => opt.MapFrom(o => o.EmployeeId.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateAgentAdmin, Domain.Agent>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(o => o.FirstName.Trim()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(o => o.LastName.Trim()))
                .ForMember(x => x.Sso, opt => opt.MapFrom(o => o.Sso.Trim()))
                .ForMember(x => x.Ssn, opt => opt.MapFrom(o => o.EmployeeId.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.Agent, AgentAdminDTO>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ReverseMap();

            CreateMap<Domain.Agent, AgentAdminDetailsDTO>()
            .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
            .ReverseMap();
        }
    }
}