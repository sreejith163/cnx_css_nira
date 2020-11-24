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

        /// <summary>Gets the skill groups.</summary>
        /// <value>The skill groups.</value>
        ISkillGroupRepository SkillGroups { get; }

        /// <summary>Gets the operation hours.</summary>
        /// <value>The operation hours.</value>
        IOperationHourRepository OperationHours { get; }

        /// <summary>Gets the time zones.</summary>
        /// <value>The time zones.</value>
        ITimezoneRepository TimeZones { get; }

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <value>
        /// The skill tags.
        /// </value>
        ISkillTagRepository SkillTags { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
