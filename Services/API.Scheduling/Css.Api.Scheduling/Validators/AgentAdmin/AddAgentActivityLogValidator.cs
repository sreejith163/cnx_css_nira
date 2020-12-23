using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentAdmin
{
    /// <summary>
    /// Validator for Add Agent Activity Log Validator
    /// </summary>
    public class AddAgentActivityLogValidator : AbstractValidator<CreateAgentActivityLog>
    {
        /// <summary>Initializes a new instance of the <see cref="AddAgentActivityLogValidator" /> class.</summary>
        public AddAgentActivityLogValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.TimeStamp).NotEmpty();
            RuleFor(x => x.ExecutedBy).NotEmpty();
            RuleFor(x => x.FieldDetails).NotEmpty();
            RuleFor(x => x.ActivityStatus).IsInEnum();
            RuleFor(x => x.ActivityOrigin).IsInEnum();
        }
    }
}