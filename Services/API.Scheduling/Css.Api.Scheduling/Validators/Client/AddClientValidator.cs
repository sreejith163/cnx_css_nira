using Css.Api.Scheduling.Models.DTO.Requests.Client;
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
            RuleFor(x => x.name).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}