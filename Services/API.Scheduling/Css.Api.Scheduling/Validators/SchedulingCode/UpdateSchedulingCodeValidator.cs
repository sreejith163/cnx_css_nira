using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.SchedulingCode
{
    /// <summary>
    /// Validator for handling the validation of update scheduling code object
    /// </summary>
    public class UpdateSchedulingCodeValidator : AbstractValidator<UpdateSchedulingCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSchedulingCodeValidator" /> class.
        /// </summary>
        public UpdateSchedulingCodeValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PriorityNumber).NotEmpty();
            RuleFor(x => x.CodeTypes).NotEmpty();
            RuleFor(x => x.IconId).NotEmpty();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}
