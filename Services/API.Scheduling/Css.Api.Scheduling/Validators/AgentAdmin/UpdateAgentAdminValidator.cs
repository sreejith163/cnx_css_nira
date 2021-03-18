using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.Agent
{
    /// <summary>
    /// Validator for handling the validation of update AgentSchedulingGroup object
    /// </summary>
    public class UpdateAgentAdminValidator : AbstractValidator<UpdateAgentAdmin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentAdminValidator"/> class.
        /// </summary>
        public UpdateAgentAdminValidator()
        {
			RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Sso).NotEmpty();            
            RuleFor(x => x.AgentSchedulingGroupId).NotEmpty();
        }
    }
}

