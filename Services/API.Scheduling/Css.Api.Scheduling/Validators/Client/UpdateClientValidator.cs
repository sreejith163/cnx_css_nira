using Css.Api.Scheduling.Models.DTO.Request.Client;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.Client
{
    /// <summary>
    /// Validator for handling the validation of update client object
    /// </summary>
    public class UpdateClientValidator : AbstractValidator<UpdateClient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateClientValidator"/> class.
        /// </summary>
        public UpdateClientValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}