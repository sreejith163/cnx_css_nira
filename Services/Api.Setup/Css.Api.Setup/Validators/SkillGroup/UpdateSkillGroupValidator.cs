using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.Validators.OperationHour;
using FluentValidation;

namespace Css.Api.Setup.Validators.SkillGroup
{
    /// <summary>
    /// Validator for handling the validation of update skill group object
    /// </summary>
    public class UpdateSkillGroupValidator : AbstractValidator<UpdateSkillGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSkillGroupValidator"/> class.
        /// </summary>
        public UpdateSkillGroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ClientLobGroupId).NotEmpty();
            RuleFor(x => x.TimezoneId).NotEmpty();
            RuleFor(x => x.FirstDayOfWeek).NotNull();
            RuleFor(x => x.OperationHour).SetValidator(new OperationHourValidator<OperationHourAttribute>());
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}
