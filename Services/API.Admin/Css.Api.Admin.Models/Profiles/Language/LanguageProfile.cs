using Css.Api.Admin.Models.Domain;
using Css.Api.Core.Models.DTO.Response;

namespace Css.Api.Admin.Models.Profiles.Language
{
    public class LanguageProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageProfile"/> class.
        /// </summary>
        public LanguageProfile()
        {
            CreateMap<CssLanguage, KeyValue>()
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
               .ReverseMap();
        }
    }
}
