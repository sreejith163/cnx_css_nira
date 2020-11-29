using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Validators.OperationHour;
using FluentValidation;

namespace Css.Api.Setup.Validators.SkillTag
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
            RuleFor(x => x.SkillGroupId).NotEmpty();
            RuleFor(x => x.OperationHour).SetValidator(new OperationHourValidator<OperationHourAttribute>());
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
