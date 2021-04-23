using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.Agent
{
    /// <summary>
    /// Validator for handling the validation of create AgentCategoryValue object
    /// </summary>
    public class CreateAgentCategoryValueValidator : AbstractValidator<CreateAgentCategoryValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAgentCategoryValueValidator"/> class.
        /// </summary>
        public CreateAgentCategoryValueValidator()
        {
			RuleFor(x => x.AgentCategoryDetails).NotEmpty();
            RuleForEach(x => x.AgentCategoryDetails)
                .ChildRules(x => x.RuleFor(x => x.StartDate).NotEmpty())
                .ChildRules(x => x.RuleFor(x => x.CategoryId).NotEmpty())
                .ChildRules(x => x.RuleFor(x => x.CategoryValue).NotEmpty());
        }
    }
}

