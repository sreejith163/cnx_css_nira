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
    public class UpdateAgentScheduleManagerChartValidator : AbstractValidator<UpdateAgentScheduleManagerChart>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentScheduleManagerChartValidator"/> class.
        /// </summary>
        public UpdateAgentScheduleManagerChartValidator()
        {
            RuleFor(x => x.ModifiedBy).NotEmpty();
            RuleFor(x => x.ModifiedUser).NotEmpty();
            RuleFor(x => x.ActivityOrigin).IsInEnum();
            RuleFor(x => x.AgentScheduleManagers).NotNull();
            RuleFor(x => x.Date).NotNull();
            RuleForEach(x => x.AgentScheduleManagers)
                .ChildRules(x => x.RuleFor(x => x.EmployeeId).NotEmpty());
            RuleForEach(x => x.AgentScheduleManagers)
                .ChildRules(x => x.RuleFor(x => x.Charts)
                .SetValidator(new AgentScheduleManagerValidator<List<ScheduleChart>>()));
        }
    }
}
