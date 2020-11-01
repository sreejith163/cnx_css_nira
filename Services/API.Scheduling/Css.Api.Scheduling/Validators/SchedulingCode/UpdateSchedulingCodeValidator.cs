using Css.Api.Scheduling.Models.DTO.Requests.SchedulingCode;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.SchedulingCode
{
    /// <summary>
    /// Validator for handling the validation of update scheduling code object
    /// </summary>
    public class UpdateSchedulingCodeValidator : AbstractValidator<CreateSchedulingCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSchedulingCodeValidator" /> class.
        /// </summary>
        public UpdateSchedulingCodeValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Priority).NotEmpty();
            RuleFor(x => x.CodeTypes).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
