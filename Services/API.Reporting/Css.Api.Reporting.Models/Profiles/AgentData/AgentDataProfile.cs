using Css.Api.Reporting.Models.DTO.Request.UDW;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using Css.Api.Reporting.Models.Profiles.ValueResolvers;

namespace Css.Api.Reporting.Models.Profiles.AgentData
{
    public class AgentDataProfile : AutoMapper.Profile
    {
        public AgentDataProfile()
        {
            CreateMap<UDWAgentDataGroup, Domain.AgentData>()
                .ForMember(x => x.Group, opt => opt.MapFrom<AgentDataGroupResolver>());
        }
    }
}
