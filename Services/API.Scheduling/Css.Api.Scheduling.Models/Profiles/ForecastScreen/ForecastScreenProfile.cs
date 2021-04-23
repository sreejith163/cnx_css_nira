using Css.Api.Scheduling.Models.DTO.Request.ForecastScreen;
using Css.Api.Scheduling.Models.DTO.Response.ForecastScreen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.Profiles.ForecastScreen
{
    public class ForecastScreenProfile : AutoMapper.Profile
    {
        public ForecastScreenProfile()
        {


            //CreateMap<CreateForecastData, Domain.ForecastData>()
             //.ForMember(x => x.Time, opt => opt.MapFrom(o => o.Time))
                   //.ForMember(x => x.ForecastedContact, opt => opt.MapFrom(o => o.ForecastedContact))
                        // .ForMember(x => x.Aht, opt => opt.MapFrom(o => o.Aht))
                             //  .ForMember(x => x.ForecastedReq, opt => opt.MapFrom(o => o.ForecastedReq))
                            //   .ForMember(x => x.ScheduledOpen, opt => opt.MapFrom(o => o.ScheduledOpen))
            // .ReverseMap();

            CreateMap<Domain.ForecastScreen, ForecastScreenDTO>()
            .ForMember(x => x.SkillGroupId, opt => opt.MapFrom(o => o.SkillGroupId))
            .ReverseMap();

            CreateMap<Domain.ForecastData, ForecastDataAtrribute>()
            .ReverseMap();

            CreateMap<Domain.ForecastScreen, CreateForecastData>()
            .ReverseMap();

            CreateMap<ForecastDataAtrribute, ImportForecastDetails>()
            .ReverseMap();



            CreateMap<UpdateForecastData, Domain.ForecastScreen>()
            .ReverseMap();

           

        }

    }
}
