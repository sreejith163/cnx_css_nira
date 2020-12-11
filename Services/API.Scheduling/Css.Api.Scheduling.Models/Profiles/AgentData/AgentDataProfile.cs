
using Css.Api.Scheduling.Models.DTO.Request.AgentData;

namespace Css.Api.Scheduling.Models.Profiles.AgentData
{
    public class AgentDataProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentDataProfile"/> class.
        /// </summary>
        public AgentDataProfile()
        {
            CreateMap<Domain.AgentData, AgentDataAttribute>()
               .ReverseMap();
        }
    }
}

