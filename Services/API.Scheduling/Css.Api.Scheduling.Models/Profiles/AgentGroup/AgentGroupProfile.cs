using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;

namespace Css.Api.Scheduling.Models.Profiles.AgentGroup
{
    public class AgentGroupProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentGroupProfile"/> class.
        /// </summary>
        public AgentGroupProfile()
        {
            CreateMap<Domain.AgentGroup, AgentGroupAttribute>()
               .ReverseMap();
        }
    }
}