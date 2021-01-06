﻿using Css.Api.Scheduling.Models.Domain;
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
            RuleFor(x => x.AgentScheduleCharts).NotEmpty();
            RuleFor(x => x.AgentScheduleCharts).SetValidator(new ScheduleChartUniqueDaysCollectionValidator());
            RuleForEach(x => x.AgentScheduleCharts).SetValidator(new AgentScheduleChartValidator<AgentScheduleChart>());
        }
    }
}
