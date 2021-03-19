using AutoMapper;
using Css.Api.Reporting.Models.DTO.Processing;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Profiles.ValueResolvers
{
    public class TimezoneOffsetStringResolver : IValueResolver<Domain.Timezone, TimezoneDetails, string>
    {
        public string Resolve(Domain.Timezone source, TimezoneDetails destination, string destMember, ResolutionContext context)
        {
            string offsetStringValue = "UTC";
            var utcOffset = source.UtcOffset.ToString().TrimEnd('0').Replace(":", "");
            if(!utcOffset.Contains("-"))
            {
                offsetStringValue = offsetStringValue + "+" + utcOffset;
            }
            else
            {
                offsetStringValue = offsetStringValue + utcOffset;
            }

            return context.Mapper.Map<string>(offsetStringValue);
        }
    }
}
