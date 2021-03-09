using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using FluentValidation;

namespace Css.Api.Admin.Validators.AgentCategory
{
    /// <summary>
    /// Validator for handling the validation of add AgentCategory object
    /// </summary>
    public class AddAgentCategoryValidator : AbstractValidator<CreateAgentCategory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddAgentCategoryValidator"/> class.
        /// </summary>
        public AddAgentCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DataTypeId).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}