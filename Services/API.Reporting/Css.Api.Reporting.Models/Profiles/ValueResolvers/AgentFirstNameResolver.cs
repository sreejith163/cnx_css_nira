using AutoMapper;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Profiles.ValueResolvers
{
    public class AgentFirstNameResolver : IValueResolver<UDWAgentAttributes, Domain.Agent,string>
    {
        public string Resolve(UDWAgentAttributes source, Domain.Agent destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.Name))
            {
                return context.Mapper.Map<string>(source.FirstName);
            }

            if(source.Name.Contains(','))
            {
                var name_split = source.Name.Split(',');
                return context.Mapper.Map<string>(name_split.Length > 1 ?name_split[1] : string.Empty);
            }
            return context.Mapper.Map<string>(source.Name.Split().Length > 1 ? source.Name.Split()[0] : string.Empty);
        }
    }
}
