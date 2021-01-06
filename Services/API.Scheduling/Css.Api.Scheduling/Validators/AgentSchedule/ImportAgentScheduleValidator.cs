using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for handling the validation of update agent schedule chart
    /// </summary>
    public class ImportAgentScheduleValidator : AbstractValidator<ImportAgentSchedule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportAgentScheduleValidator"/> class.
        /// </summary>
        public ImportAgentScheduleValidator()
        {
            RuleFor(x => x.ModifiedBy).NotEmpty();
            RuleFor(x => x.ImportAgentScheduleCharts).NotEmpty();
            RuleForEach(x => x.ImportAgentScheduleCharts)
                .ChildRules(x => x.RuleFor(x => x.EmployeeId).NotEmpty());
            RuleForEach(x => x.ImportAgentScheduleCharts)
                .ChildRules(x => x.RuleFor(x => x.AgentScheduleCharts)
                .SetValidator(new ScheduleChartUniqueDaysCollectionValidator()));
            RuleForEach(x => x.ImportAgentScheduleCharts)
                .ChildRules(x => x.RuleFor(x => x.AgentScheduleCharts)
                .SetValidator(new AgentScheduleChartValidator<AgentScheduleChart>()));
        }
    }
}
