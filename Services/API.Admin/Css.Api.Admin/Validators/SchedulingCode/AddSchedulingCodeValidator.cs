using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using FluentValidation;

namespace Css.Api.Admin.Validators.SchedulingCode
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
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PriorityNumber).NotEmpty();
            RuleFor(x => x.SchedulingTypeCode).NotEmpty();
            RuleFor(x => x.IconId).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
