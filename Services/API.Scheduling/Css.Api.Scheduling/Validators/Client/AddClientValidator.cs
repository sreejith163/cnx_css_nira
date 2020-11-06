using Css.Api.Scheduling.Models.DTO.Request.Client;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.Client
{
    /// <summary>
    /// Validator for handling the validation of add client object
    /// </summary>
    public class AddClientValidator : AbstractValidator<CreateClient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddClientValidator"/> class.
        /// </summary>
        public AddClientValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}