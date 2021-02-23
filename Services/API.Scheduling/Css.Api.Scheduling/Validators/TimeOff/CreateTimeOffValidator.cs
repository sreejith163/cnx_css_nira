using Css.Api.Scheduling.Models.DTO.Request.TimeOff;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.Agent
{
    /// <summary>
    /// Validator for handling the validation of update AgentSchedulingGroup object
    /// </summary>
    public class CreateTimeOffValidator : AbstractValidator<CreateTimeOff>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTimeOffValidator"/> class.
        /// </summary>
        public CreateTimeOffValidator()
        {
			RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.SchedulingCodeId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();   
            RuleFor(x => x.AllowDayRequest).NotEmpty();   
            RuleFor(x => x.FTEDayLength).NotEmpty();   
            RuleFor(x => x.FirstDayOfWeek).NotEmpty();   
            RuleFor(x => x.ForceOffDaysBeforeWeek).NotEmpty();
            RuleFor(x => x.ForceOffDaysAfterWeek).NotEmpty();
            RuleFor(x => x.DeSelectedTime).IsInEnum();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}

