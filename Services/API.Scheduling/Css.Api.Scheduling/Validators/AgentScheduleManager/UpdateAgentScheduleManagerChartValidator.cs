using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Validators.AgentScheduleManager
{
    /// <summary>
    /// Custom validator for agent chart time range
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FluentValidation.Validators.PropertyValidator" />
    public class UpdateAgentScheduleManagerChartValidator<T> : PropertyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAgentScheduleManagerChartValidator{T}" /> class.
        /// </summary>
        public UpdateAgentScheduleManagerChartValidator()
            : base("{PropertyName} must be provided.")
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <inheritdoc />
        public override IEnumerable<ValidationFailure> Validate(PropertyValidatorContext context)
        {
            var validationFailures = new List<ValidationFailure>();
            var agentScheduleManagerCharts = context.PropertyValue as List<AgentScheduleManagerChartDTO>;

            foreach (var agentScheduleManagerChart in agentScheduleManagerCharts)
            {
                if (agentScheduleManagerCharts.FindAll(x => x.Date == agentScheduleManagerChart.Date)?.Count > 1)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Range", "Schedule cannot have overlapping dates"));
                    return validationFailures;
                }
            }

            return validationFailures;
        }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is valid; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            throw new NotImplementedException();
        }
    }
}
