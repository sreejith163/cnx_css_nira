using System.Threading.Tasks;

namespace Css.Api.Setup.Repository.Interfaces
{
    /// <summary>
    /// Interface for repository wrapper
    /// </summary>
    public interface IRepositoryWrapper
    {


        /// <summary>
        /// Gets the clients.
        /// </summary>
        IClientRepository Clients { get; }

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        IClientLOBGroupRepository ClientLOBGroups { get; }

        /// <summary>
        /// Gets the skill groups.
        /// </summary>
        ISkillGroupRepository SkillGroups { get; }

        /// <summary>
        /// Gets the operation hours.
        /// </summary>
        IOperationHourRepository OperationHours { get; }

        /// <summary>
        /// Gets the agent scheduling.
        /// </summary>
        IAgentSchedulingGroupRepository AgentSchedulingGroups { get; }

        /// <summary>
        /// Gets the time zones.
        /// </summary>
        ITimezoneRepository TimeZones { get; }

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        ISkillTagRepository SkillTags { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        Task<bool> SaveAsync();
    }
}
