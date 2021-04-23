using AutoMapper;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using Css.Api.Core.Models.Domain.NoSQL;
using System.Linq;
using System.Globalization;

namespace Css.Api.Reporting.Models.Profiles.ValueResolvers
{
    public class AgentDataGroupResolver : IValueResolver<UDWAgentDataGroup, Domain.AgentData, AgentGroup>
    {
        public AgentGroup Resolve(UDWAgentDataGroup source, Domain.AgentData destination, AgentGroup destMember, ResolutionContext context)
        {
            return context.Mapper.Map<AgentGroup>(new AgentGroup()
            {
                Description = source.Description,
                Value = source.Value
            });
        }
    }
}
