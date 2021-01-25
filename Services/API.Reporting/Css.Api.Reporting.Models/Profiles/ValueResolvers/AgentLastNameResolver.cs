using AutoMapper;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
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

            if (source.Name.Contains(','))
            {
                var name_split = source.Name.Split(',');
                return context.Mapper.Map<string>(name_split[0]);
            }
            return context.Mapper.Map<string>(source.Name.Split().Length > 1 ? source.Name.Split()[1] : source.Name);
        }
    }
}
