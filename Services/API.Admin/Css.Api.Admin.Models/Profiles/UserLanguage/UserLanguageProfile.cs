using Css.Api.Admin.Models.DTO.Request.UserLanguage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Profiles.UserLanguage
{
    public class UserLanguageProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserLanguageProfile"/> class.
        /// </summary>
        public UserLanguageProfile()
        {
            CreateMap<Domain.UserLanguagePreference, UserLanguagePreferenceDTO>().ReverseMap();
            CreateMap<Domain.UserLanguagePreference, UpdateUserLanguagePreferenceDTO>().ReverseMap();

        }
    }
}
