using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of update agent schedule object
    /// </summary>
    public class UpdateAgentScheduleDateRangeValidator : AbstractValidator<UpdateAgentScheduleDateRange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentScheduleDateRangeValidator"/> class.
        /// </summary>
        public UpdateAgentScheduleDateRangeValidator()
        {
            RuleFor(x => x.OldDateFrom).NotEmpty();
            RuleFor(x => x.OldDateTo).NotEmpty();
            RuleFor(x => x.NewDateFrom).NotEmpty();
            RuleFor(x => x.NewDateTo).NotEmpty();
            RuleFor(x => x.ModifiedBy).NotEmpty();
        }
    }
}