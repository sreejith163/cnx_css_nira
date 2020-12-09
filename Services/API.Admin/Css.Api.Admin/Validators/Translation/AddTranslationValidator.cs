using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using FluentValidation;

namespace Css.Api.Admin.Validators.AgentCategory
{
    /// <summary>
    /// Validator for handling the validation of add AgentCategory object
    /// </summary>
    public class AddTranslationValidator : AbstractValidator<CreateLanguageTranslation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddTranslationValidator"/> class.
        /// </summary>
        public AddTranslationValidator()
        {
            RuleFor(x => x.LanguageId).NotEmpty();
            RuleFor(x => x.MenuId).NotEmpty();
            RuleFor(x => x.VariableId).NotEmpty();
            RuleFor(x => x.Translation).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}