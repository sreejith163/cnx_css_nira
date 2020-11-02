using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.ClientLOBGroup
{
    /// <summary>Validator for handling the validation of update client lob group object</summary>
    public class UpdateClientLOBGroupValidator : AbstractValidator<UpdateClientLOBGroup>
    {
        /// <summary>Initializes a new instance of the <see cref="UpdateClientLOBGroupValidator" /> class.</summary>
        public UpdateClientLOBGroupValidator()
        {
            RuleFor(x => x.name).NotEmpty();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}
