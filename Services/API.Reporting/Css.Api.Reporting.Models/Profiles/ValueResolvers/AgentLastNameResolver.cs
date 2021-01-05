using AutoMapper;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Profiles.ValueResolvers
{
    public class AgentLastNameResolver : IValueResolver<UDWAgentAttributes, Domain.Agent, string>
    {
        public string Resolve(UDWAgentAttributes source, Domain.Agent destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.Name))
            {
                return context.Mapper.Map<string>(source.LastName);
            }
            return context.Mapper.Map<string>(source.Name.Split().Length > 1 ? source.Name.Split()[1] : source.Name);
        }
    }
}
