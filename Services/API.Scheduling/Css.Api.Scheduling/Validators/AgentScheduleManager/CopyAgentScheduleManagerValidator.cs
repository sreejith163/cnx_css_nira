using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of update agent schedule managerobject
    /// </summary>
    public class CopyAgentScheduleManagerValidator : AbstractValidator<CopyAgentScheduleManager>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAgentScheduleManagerValidator"/> class.
        /// </summary>
        public CopyAgentScheduleManagerValidator()
        {
            RuleFor(x => x.AgentSchedulingGroupId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.ModifiedUser).NotEmpty();
            RuleFor(x => x.ActivityOrigin).IsInEnum();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}