using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of import agent schedule object
    /// </summary>
    public class AgentValidator : AbstractValidator<CreateAgentAdmin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentValidator"/> class.
        /// </summary>
        public AgentValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Sso).NotEmpty();
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.ClientLobGroupId).NotEmpty();
            RuleFor(x => x.SkillGroupId).NotEmpty();
            RuleFor(x => x.SkillTagId).NotEmpty();
        }
    }
}