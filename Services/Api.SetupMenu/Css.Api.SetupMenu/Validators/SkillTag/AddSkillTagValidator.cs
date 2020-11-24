using Css.Api.SetupMenu.Models.DTO.Request.SkillTag;
using FluentValidation;

namespace Css.Api.SetupMenu.Validators.SkillTag
{
    /// <summary>
    /// Validator for handling the validation of add skill tag object
    /// </summary>
    public class AddSkillTagValidator : AbstractValidator<CreateSkillTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddSkillTagValidator"/> class.
        /// </summary>
        public AddSkillTagValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.SkillGroupId).NotEmpty();;
            RuleFor(x => x.OperationHour).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
