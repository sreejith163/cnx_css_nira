﻿using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using System;
using Css.Api.Core.Models.Domain;

namespace Css.Api.Scheduling.Models.Profiles.AgentAdmin
{
    public class AgentAdminProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentAdminProfile" /> class.</summary>
        public AgentAdminProfile()
        {
            CreateMap<CreateAgentAdmin, NoSQL.Agent>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(o => o.FirstName.Trim()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(o => o.LastName.Trim()))
                .ForMember(x => x.Sso, opt => opt.MapFrom(o => o.Sso.Trim()))
                .ForMember(x => x.Ssn, opt => opt.MapFrom(o => o.EmployeeId))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateAgentAdmin, NoSQL.Agent>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(o => o.FirstName.Trim()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(o => o.LastName.Trim()))
                .ForMember(x => x.Sso, opt => opt.MapFrom(o => o.Sso.Trim()))
                .ForMember(x => x.Ssn, opt => opt.MapFrom(o => o.EmployeeId))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<NoSQL.Agent, AgentAdminDTO>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ReverseMap();

            CreateMap<NoSQL.Agent, AgentAdminDetailsDTO>()
            .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
            .ReverseMap();

            CreateMap<NoSQL.AgentData, AgentDataAttribute>()
               .ReverseMap();

            CreateMap<NoSQL.AgentGroup, AgentGroupAttribute>()
              .ReverseMap();

            CreateMap<NoSQL.AgentPto, AgentPtoAttribute>()
              .ReverseMap();

            CreateMap<NoSQL.ActivityLog, CreateAgentActivityLog>()
             .ReverseMap();

            CreateMap<NoSQL.ActivityLog, ActivityLogDTO>()
            .ReverseMap();
        }
    }
}