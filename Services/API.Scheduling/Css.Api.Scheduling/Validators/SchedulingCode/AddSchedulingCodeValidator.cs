using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.SchedulingCode
{
    /// <summary>
    /// Validator for handling the validation of add scheduling code object
    /// </summary>
    public class AddSchedulingCodeValidator : AbstractValidator<CreateSchedulingCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddSchedulingCodeValidator"/> class.
        /// </summary>
        public AddSchedulingCodeValidator()
        {
            RuleFor(x => x.RefId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Priority).NotEmpty();
            RuleFor(x => x.CodeTypes).NotEmpty();
            RuleFor(x => x.IconId).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
