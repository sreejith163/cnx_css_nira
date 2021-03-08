using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of update agent schedule object
    /// </summary>
    public class CopyAgentScheduleValidator : AbstractValidator<CopyAgentSchedule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAgentScheduleValidator"/> class.
        /// </summary>
        public CopyAgentScheduleValidator()
        {
            RuleFor(x => x.AgentSchedulingGroupId).NotEmpty();
            RuleFor(x => x.DateFrom).NotEmpty();
            RuleFor(x => x.DateTo).NotEmpty();
            RuleFor(x => x.ModifiedUser).NotEmpty();
            RuleFor(x => x.ActivityOrigin).IsInEnum();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}