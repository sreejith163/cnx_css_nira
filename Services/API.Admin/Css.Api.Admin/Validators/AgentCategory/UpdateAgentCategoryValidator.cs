using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using FluentValidation;

namespace Css.Api.Admin.Validators.AgentCategory
{
    /// <summary>
    /// Validator for handling the validation of add AgentCategory object
    /// </summary>
    public class UpdateAgentCategoryValidator : AbstractValidator<UpdateAgentCategory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentCategoryValidator"/> class.
        /// </summary>
        public UpdateAgentCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DataTypeId).NotEmpty();
            RuleFor(x => x.DataTypeMinValue).NotEmpty();
            RuleFor(x => x.DataTypeMaxValue).NotEmpty();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}