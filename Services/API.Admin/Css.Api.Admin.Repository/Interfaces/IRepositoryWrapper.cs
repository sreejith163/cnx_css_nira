using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    /// <summary>
    /// Interface for repository wrapper
    /// </summary>
    public interface IRepositoryWrapper
    { 
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
        /// <value>
        /// The language.
        /// </value>
        ICssLanguageRepository CssLanguage { get; }		

        /// <summary>
        /// Gets the CSS menu.
        /// </summary>
        /// <value>
        /// The CSS menu.
        /// </value>
        ICssMenuRepository CssMenu { get; }

        /// <summary>
        /// Gets the CSS variable.
        /// </summary>
        /// <value>
        /// The CSS variable.
        /// </value>
        ICssVariableRepository CssVariable { get; }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <value>
        /// The language translation.
        /// </value>
        ILanguageTranslationRepository LanguageTranslation { get; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();
    }
}
