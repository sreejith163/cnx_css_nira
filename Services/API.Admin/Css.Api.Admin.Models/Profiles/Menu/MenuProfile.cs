using Css.Api.Admin.Models.Domain;
using Css.Api.Core.Models.DTO.Response;

namespace Css.Api.Admin.Models.Profiles.Menu
{
    public class MenuProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuProfile"/> class.
        /// </summary>
        public MenuProfile()
        {
            CreateMap<CssMenu, KeyValue>()
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
               .ReverseMap();
        }
    }
}
