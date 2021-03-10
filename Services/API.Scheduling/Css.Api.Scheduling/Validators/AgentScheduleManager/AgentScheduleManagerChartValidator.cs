using Css.Api.Core.Models.Domain.NoSQL;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// custom validator for agent schedule manager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FluentValidation.Validators.PropertyValidator" />
    public class AgentScheduleManagerChartValidator<T> : PropertyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagerChartValidator{T}" /> class.
        /// </summary>
        public AgentScheduleManagerChartValidator()
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
            var item = context.PropertyValue as List<AgentScheduleManagerChart>;

            foreach (var chart in item)
            {
                if (chart.SchedulingCodeId == 0)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "SchedulingCodeId should not be empty"));
                    return validationFailures;
                }
                else if (chart.StartTime >= chart.EndTime)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "Start time cannot be greater than end time"));
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
