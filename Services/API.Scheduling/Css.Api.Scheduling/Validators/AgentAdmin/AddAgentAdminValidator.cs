using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.Agent
{
    /// <summary>
    /// Validator for agent admin
    /// </summary>
    public class AddAgentAdminValidator : AbstractValidator<CreateAgentAdmin>
    {
        /// <summary>Initializes a new instance of the <see cref="AddAgentAdminValidator" /> class.</summary>
        public AddAgentAdminValidator()
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
