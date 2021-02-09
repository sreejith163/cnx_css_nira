using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of update agent schedule chart
    /// </summary>
    public class UpdateAgentScheduleChartValidator : AbstractValidator<UpdateAgentScheduleChart>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentScheduleChartValidator"/> class.
        /// </summary>
        public UpdateAgentScheduleChartValidator()
        {
            RuleFor(x => x.ModifiedBy).NotEmpty();
            RuleFor(x => x.ModifiedUser).NotEmpty();
            RuleFor(x => x.ActivityOrigin).IsInEnum();
            RuleFor(x => x.AgentScheduleCharts).NotNull();
            RuleFor(x => x.AgentScheduleCharts).SetValidator(new ScheduleChartUniqueDaysCollectionValidator());
            RuleForEach(x => x.AgentScheduleCharts).SetValidator(new AgentScheduleChartValidator<AgentScheduleChart>());
        }
    }
}
