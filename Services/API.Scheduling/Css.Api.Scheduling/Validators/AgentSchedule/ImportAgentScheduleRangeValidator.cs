using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Custom validator for import agent schedule range
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FluentValidation.Validators.PropertyValidator" />
    public class ImportAgentScheduleRangeValidator<T> : PropertyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportAgentScheduleRangeValidator{T}" /> class.
        /// </summary>
        public ImportAgentScheduleRangeValidator()
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
            var scheduleRanges = context.PropertyValue as List<ImportAgentScheduleRange>;

            foreach (var scheduleRange in scheduleRanges)
            {
                if (scheduleRanges.Exists(x => scheduleRange.DateFrom != x.DateFrom && scheduleRange.DateTo != x.DateTo &&
                                               scheduleRange.DateFrom < x.DateTo && scheduleRange.DateTo > x.DateFrom))
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

