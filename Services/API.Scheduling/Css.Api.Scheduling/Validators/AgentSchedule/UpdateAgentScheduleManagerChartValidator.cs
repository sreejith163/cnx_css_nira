﻿using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation;

namespace Css.Api.Scheduling.Validators.AgentSchedule
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
            RuleFor(x => x.AgentScheduleManagers).NotEmpty();
            RuleForEach(x => x.AgentScheduleManagers)
                .ChildRules(x => x.RuleFor(x => x.EmployeeId).NotEmpty());
            RuleForEach(x => x.AgentScheduleManagers)
                .ChildRules(x => x.RuleFor(x => x.AgentScheduleManagerChart)
                .SetValidator(new AgentScheduleManagerValidator<AgentScheduleManagerChart>()));
        }
    }
}