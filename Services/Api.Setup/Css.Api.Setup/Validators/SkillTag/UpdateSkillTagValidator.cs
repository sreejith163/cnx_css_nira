using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Validators.OperationHour;
using FluentValidation;

namespace Css.Api.Setup.Validators.SkillTag
{
    /// <summary>
    /// Validator for handling the validation of update skill tag object
    /// </summary>
    public class UpdateSkillTagValidator : AbstractValidator<UpdateSkillTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSkillTagValidator"/> class.
        /// </summary>
        public UpdateSkillTagValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.SkillGroupId).NotEmpty();
            RuleFor(x => x.OperationHour).SetValidator(new OperationHourValidator<OperationHourAttribute>());
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}
