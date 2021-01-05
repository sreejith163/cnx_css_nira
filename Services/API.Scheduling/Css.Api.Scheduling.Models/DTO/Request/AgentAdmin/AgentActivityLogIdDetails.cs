using MongoDB.Bson;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class AgentActivityLogIdDetails
    {
        /// <summary>Gets or sets the agent activity log identifier.</summary>
        /// <value>The agent activity log identifier.</value>
        public ObjectId AgentActivityLogId { get; set; }
    }
}
