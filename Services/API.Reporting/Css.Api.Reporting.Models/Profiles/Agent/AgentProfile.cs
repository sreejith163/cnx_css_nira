using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.Profiles.ValueResolvers;
using System;
using System.Collections.Generic;
using System.Text;

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
                .ForMember(x => x.Mu, opt => opt.MapFrom(o => o.MU))
                .ForMember(x => x.SenDate, opt =>
                {
                    opt.PreCondition(o => {
                        DateTime dt;
                        return o.SenDate != null
                            && (DateTime.TryParse(String.Join('-', o.SenDate.Year, o.SenDate.Month, o.SenDate.Day), out dt));
                    });
                    opt.MapFrom(o => new DateTime(o.SenDate.Year, o.SenDate.Month, o.SenDate.Day));
                })
                .ForMember(x => x.SenExt, opt =>
                {
                    opt.PreCondition(o =>
                    {
                        DateTime dt;
                        return o.SenExt != null
                            && (DateTime.TryParse(String.Join('-', o.SenExt.Year, o.SenExt.Month, o.SenExt.Day), out dt));
                    });
                    opt.MapFrom(o => new DateTime(o.SenExt.Year, o.SenExt.Month, o.SenExt.Day));
                });

            CreateMap<UDWAgentUpdate, Domain.Agent>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom<AgentFirstNameResolver>())
                .ForMember(x => x.LastName, opt => opt.MapFrom<AgentLastNameResolver>())
                .ForMember(x => x.Ssn, opt => opt.MapFrom(o => o.SSN))
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => "UDW Import"))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.Mu, opt => opt.MapFrom(o => o.MU))
                .ForMember(x => x.SenDate, opt =>
                {
                    opt.PreCondition(o => o.SenDate != null);
                    opt.MapFrom(o => new DateTime(o.SenDate.Year, o.SenDate.Month, o.SenDate.Day));
                })
                .ForMember(x => x.SenExt, opt =>
                {
                    opt.PreCondition(o => o.SenExt != null);
                    opt.MapFrom(o => new DateTime(o.SenExt.Year, o.SenExt.Month, o.SenExt.Day));
                });
                   
        }
    }
}
