using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Validators.OperationHour;
using FluentValidation;

namespace Css.Api.Setup.Validators.AgentSchedulingGroup
{
    /// <summary>
    /// Validator for handling the validation of add SchedulingGroup object
    /// </summary>
    public class AddAgentSchedulingGroupValidator : AbstractValidator<CreateAgentSchedulingGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddAgentSchedulingGroupValidator"/> class.
        /// </summary>
        public AddAgentSchedulingGroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.SkillTagId).NotEmpty();
            RuleFor(x => x.TimezoneId).NotEmpty();
            RuleFor(x => x.FirstDayOfWeek).NotNull();
            RuleFor(x => x.OperationHour).SetValidator(new OperationHourValidator<OperationHourAttribute>());
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
