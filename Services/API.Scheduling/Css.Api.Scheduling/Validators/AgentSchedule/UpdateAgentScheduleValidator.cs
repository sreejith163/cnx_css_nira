using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of update agent schedule object
    /// </summary>
    public class UpdateAgentScheduleValidator : AbstractValidator<UpdateAgentSchedule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentScheduleValidator"/> class.
        /// </summary>
        public UpdateAgentScheduleValidator()
        {
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}