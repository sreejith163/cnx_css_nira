using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
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
            RuleFor(x => x.AgentScheduleType).IsInEnum();
            RuleFor(x => x.AgentScheduleCharts).SetValidator(new ScheduleChartUniqueDaysCollectionValidator())
                .When(x => x.AgentScheduleType == AgentScheduleType.SchedulingTab);
            RuleForEach(x => x.AgentScheduleCharts).SetValidator(new AgentScheduleChartValidator<AgentScheduleChart>())
                .When(x => x.AgentScheduleType == AgentScheduleType.SchedulingTab);
            RuleFor(x => x.AgentScheduleManagerChart).SetValidator(new AgentScheduleManagerValidator<AgentScheduleManagerChart>())
                .When(x => x.AgentScheduleType == AgentScheduleType.SchedulingMangerTab);
        }
    }
}
