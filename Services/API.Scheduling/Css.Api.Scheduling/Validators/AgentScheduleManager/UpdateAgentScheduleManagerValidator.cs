using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Validators.AgentSchedule;
using FluentValidation;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Validators.AgentScheduleManager
{
    /// <summary>
    /// Validator for handling the validation of update agent schedule chart
    /// </summary>
    public class UpdateAgentScheduleManagerValidator : AbstractValidator<UpdateAgentScheduleManager>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentScheduleManagerValidator"/> class.
        /// </summary>
        public UpdateAgentScheduleManagerValidator()
        {
            RuleFor(x => x.ModifiedBy).NotEmpty();
            RuleFor(x => x.ModifiedUser).NotEmpty();
            RuleFor(x => x.ActivityOrigin).IsInEnum();
            RuleFor(x => x.ScheduleManagers).NotNull();
            RuleForEach(x => x.ScheduleManagers)
                .ChildRules(x => x.RuleFor(x => x.EmployeeId).NotEmpty())
                .ChildRules(x => x.RuleFor(x => x.AgentScheduleManagerCharts).SetValidator(new UpdateAgentScheduleManagerChartValidator<AgentScheduleManagerChartDTO>()))
                .ChildRules(x => x.RuleForEach(x => x.AgentScheduleManagerCharts)
                                        .ChildRules(x => x.RuleFor(x => x.Date).NotEmpty())
                                        .ChildRules(x => x.RuleFor(x => x.Charts)
                                        .SetValidator(new AgentScheduleManagerChartValidator<List<AgentScheduleManagerChart>>()))
                );
        }
    }
}
