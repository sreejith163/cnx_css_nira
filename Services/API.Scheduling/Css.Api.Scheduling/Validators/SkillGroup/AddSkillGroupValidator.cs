using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.SkillGroup
{
    /// <summary>
    /// Validator for handling the validation of add skill group object
    /// </summary>
    public class AddSkillGroupValidator : AbstractValidator<CreateSkillGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddSkillGroupValidator"/> class.
        /// </summary>
        public AddSkillGroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ClientLobGroupId).NotEmpty();
            RuleFor(x => x.TimezoneId).NotEmpty();
            RuleFor(x => x.FirstDayOfWeek).NotNull();
            RuleFor(x => x.OperationHour).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
