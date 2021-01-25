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
            if(source.Description.ToLower().Split().ToList().Contains("date"))
            {
                string dateValue = source.Value;
                if(!string.IsNullOrWhiteSpace(dateValue))
                {
                    dateValue = DateTime.ParseExact(dateValue, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }
                return context.Mapper.Map<AgentGroup>(new AgentGroup() { 
                    Description = source.Description,
                    Value = dateValue
                });
            }
            return context.Mapper.Map<AgentGroup>(new AgentGroup()
            {
                Description = source.Description,
                Value = source.Value
            });
        }
    }
}
