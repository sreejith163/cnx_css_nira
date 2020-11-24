using Css.Api.SetupMenu.Models.DTO.Request.OperationHour;

namespace Css.Api.SetupMenu.Models.Profiles.OperatingHours
{
    public class OperatingHourProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingHourProfile"/> class.
        /// </summary>
        public OperatingHourProfile()
        {
            CreateMap<Domain.OperationHour, OperationHourAttribute>()
               .ReverseMap();
        }
    }
}
