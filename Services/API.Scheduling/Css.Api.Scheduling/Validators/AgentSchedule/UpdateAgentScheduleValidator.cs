using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using FluentValidation;

namespace Css.Api.Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of add AgentCategory object
    /// </summary>
    public class UpdateAgentScheduleValidator : AbstractValidator<UpdateAgentSchedule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentScheduleValidator"/> class.
        /// </summary>
        public UpdateAgentScheduleValidator()
        {
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.ModifiedBy).NotEmpty();
            RuleFor(x => x.AgentScheduleType).IsInEnum();
        }
    }
}