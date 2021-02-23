using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    /// <summary>
    /// Interface for repository wrapper
    /// </summary>
    public interface IRepositoryWrapper
    {
        /// <summary>
        /// Gets the user language.
        /// </summary>
        IUserLanguageRepository UserLanguage { get; }

        /// <summary>
        /// Gets the user permissions.
        /// </summary>
        IUserPermissionRepository UserPermissions { get; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        IRoleRepository Roles { get; }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        ISchedulingCodeRepository SchedulingCodes { get; }

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        ISchedulingCodeIconRepository SchedulingCodeIcons { get; }

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        ISchedulingCodeTypeRepository SchedulingCodeTypes { get; }

        /// <summary>
        /// Gets the scheduling type codes.
        /// </summary>
        ISchedulingTypeCodeRepository SchedulingTypeCodes { get; }

        /// <summary>Gets the agent categories.</summary>
        /// <value>The agent categories.</value>
        IAgentCategoryRepository AgentCategories { get; }       
		
		/// <summary>
        /// Gets the language.
        /// </summary>
        ICssLanguageRepository CssLanguage { get; }		

        /// <summary>
        /// Gets the CSS menu.
        /// </summary>
        ICssMenuRepository CssMenu { get; }

        /// <summary>
        /// Gets the CSS variable.
        /// </summary>
        ICssVariableRepository CssVariable { get; }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        ILanguageTranslationRepository LanguageTranslation { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
