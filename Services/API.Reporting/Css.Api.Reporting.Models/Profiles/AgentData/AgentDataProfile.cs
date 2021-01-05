using Css.Api.Reporting.Models.DTO.Request.UDW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Profiles.AgentData
{
    public class AgentDataProfile : AutoMapper.Profile
    {
        public AgentDataProfile()
        {
            CreateMap<UDWAgentDataGroup, Domain.AgentData>()
                .ForMember(x => x.Group, opt => opt.MapFrom(o => new Domain.AgentGroup()
                {
                    Description = o.Description,
                    Value = o.Value
                }));
        }
    }
}
