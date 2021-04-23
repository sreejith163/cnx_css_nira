using Css.Api.Reporting.Models.DTO.Processing;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using Css.Api.Reporting.Models.Profiles.ValueResolvers;
using Css.Api.Core.Utilities.Extensions;

namespace Css.Api.Reporting.Models.Profiles.Timezone
{
    public class TimezoneProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneProfile"/> class.
        /// </summary>
        public TimezoneProfile()
        {
            CreateMap<Domain.Timezone, TimezoneDetails>()
                .ForMember(x => x.TimezoneId, opt => opt.MapFrom(o => o.TimezoneId))
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name))
                .ForMember(x => x.TimezoneValue, opt => opt.MapFrom(o => o.Abbreviation))
                .ForMember(x => x.TimezoneOffset, opt => opt.MapFrom(o => TimezoneHelper.GetOffset(o.Abbreviation).Value))
                .ForMember(x => x.TimezoneOffsetValue, opt => opt.MapFrom<TimezoneOffsetStringResolver>());
        }
    }
}
