using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.Profiles.ValueResolvers;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Css.Api.Core.Models.Enums;

namespace Css.Api.Reporting.Models.Profiles.Agent
{
    public class AgentProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentProfile"/> class.
        /// </summary>
        public AgentProfile()
        {
            CreateMap<UDWAgent, Domain.Agent>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom<AgentFirstNameResolver>())
                .ForMember(x => x.LastName, opt => opt.MapFrom<AgentLastNameResolver>())
                .ForMember(x => x.Ssn, opt => opt.MapFrom(o => o.SSN))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => "UDW Import"))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.Mu, opt => opt.MapFrom(o => o.MU))
                .ForMember(x => x.Origin, opt => opt.MapFrom(o => ActivityOrigin.UDW))
                .ForMember(x => x.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(x => x.HireDate, opt =>
                {
                    opt.PreCondition(o => {
                        DateTime dt;
                        if (o.SenDate == null)
                        {
                            return false;
                        }
                        else if (o.SenDate.Day == 0 && o.SenDate.Month == 0 && o.SenDate.Year == 0)
                        {
                            o.SenDate = null;
                            return false;
                        }
                        return DateTime.TryParse(String.Join('-', o.SenDate.Year, o.SenDate.Month, o.SenDate.Day), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dt);
                    });
                    opt.MapFrom(o => new DateTime(o.SenDate.Year, o.SenDate.Month, o.SenDate.Day, 0, 0, 0, DateTimeKind.Utc));
                })
                .ForMember(x => x.SenExt, opt =>
                {
                    opt.PreCondition(o => o.SenExt.HasValue && o.SenExt.Value > 0 && o.SenExt.Value < 10000);
                    opt.MapFrom(o => DateTime.UtcNow.Date.AddDays((double)o.SenExt));
                });

            CreateMap<UDWAgentUpdate, Domain.Agent>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom<AgentFirstNameResolver>())
                .ForMember(x => x.LastName, opt => opt.MapFrom<AgentLastNameResolver>())
                .ForMember(x => x.Ssn, opt => opt.MapFrom(o => o.SSN))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => "UDW Import"))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.Mu, opt => opt.MapFrom(o => o.MU))
                .ForMember(x => x.Origin, opt => opt.MapFrom(o => ActivityOrigin.UDW))
                .ForMember(x => x.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(x => x.HireDate, opt =>
                {
                    opt.PreCondition(o => {
                        DateTime dt;
                        if (o.SenDate == null)
                        {
                            return false;
                        }
                        else if (o.SenDate.Day == 0 && o.SenDate.Month == 0 && o.SenDate.Year == 0)
                        {
                            o.SenDate = null;
                            return false;
                        }
                        return DateTime.TryParse(String.Join('-', o.SenDate.Year, o.SenDate.Month, o.SenDate.Day), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal ,out dt);
                    });
                    opt.MapFrom(o => new DateTime(o.SenDate.Year, o.SenDate.Month, o.SenDate.Day, 0, 0, 0, DateTimeKind.Utc));
                })
                .ForMember(x => x.SenExt, opt =>
                {
                    opt.PreCondition(o => o.SenExt.HasValue && o.SenExt.Value > 0 && o.SenExt.Value < 10000);
                    opt.MapFrom(o => DateTime.UtcNow.Date.AddDays((double)o.SenExt));
                });
                   
        }
    }
}
