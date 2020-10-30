using Css.Api.Scheduling.Models.DTO.Requests.Client;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.Client
{
    /// <summary>
    /// Validator for handling the validation of client id object
    /// </summary>
    public class ClientIdDetailsValidator : AbstractValidator<ClientIdDetails>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdDetailsValidator"/> class.
        /// </summary>
        public ClientIdDetailsValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
        }
    }
}