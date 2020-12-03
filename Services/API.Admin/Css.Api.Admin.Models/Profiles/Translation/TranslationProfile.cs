using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Response.LanguageTranslation;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Profiles.Translation
{
    public class TranslationProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationProfile"/> class.
        /// </summary>
        public TranslationProfile()
        {
            CreateMap<CssLanguage, KeyValue>()
                .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
                .ReverseMap();

            CreateMap<CssVariable, KeyValue>()
               .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
               .ReverseMap();

            CreateMap<CssMenu, KeyValue>()
               .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
               .ReverseMap();

            CreateMap<LanguageTranslation, LanguageTranslationDTO>()
                .ReverseMap();

            CreateMap<CreateLanguageTranslation, LanguageTranslation>()
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description.Trim()))
                .ForMember(x => x.Translation, opt => opt.MapFrom(o => o.Translation.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateLanguageTranslation, LanguageTranslation>()
                .ForMember(x => x.Translation, opt => opt.MapFrom(o => o.Translation.Trim()))
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}
