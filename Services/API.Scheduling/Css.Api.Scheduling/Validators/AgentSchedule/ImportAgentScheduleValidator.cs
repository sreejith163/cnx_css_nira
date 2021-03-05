using Css.Api.Core.Models.Domain.NoSQL;
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
            RuleFor(x => x.ModifiedUser).NotEmpty();
            RuleFor(x => x.ActivityOrigin).IsInEnum();
            RuleFor(x => x.ImportAgentScheduleCharts).NotNull();
            RuleForEach(x => x.ImportAgentScheduleCharts)
                .ChildRules(x => x.RuleFor(x => x.EmployeeId).NotEmpty())
                .ChildRules(x => x.RuleFor(x => x.Ranges).NotEmpty())
                .ChildRules(x =>  x.RuleForEach(x => x.Ranges)
                                        .ChildRules(x => x.RuleFor(x => x.DateFrom).NotEmpty())
                                        .ChildRules(x => x.RuleFor(x => x.DateTo).NotEmpty())
                                        .ChildRules(x => x.RuleFor(x => x.AgentScheduleCharts)
                                        .SetValidator(new ScheduleChartUniqueDaysCollectionValidator()))
                                        .ChildRules(x => x.RuleForEach(x => x.AgentScheduleCharts)
                                        .SetValidator(new AgentScheduleChartValidator<AgentScheduleChart>()))
          );
        }
    }
}
