using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of date range object
    /// </summary>
    public class DateRangeValidator : AbstractValidator<DateRange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeValidator"/> class.
        /// </summary>
        public DateRangeValidator()
        {
            RuleFor(x => x.DateFrom).NotEmpty();
            RuleFor(x => x.DateTo).NotEmpty();
        }
    }
}