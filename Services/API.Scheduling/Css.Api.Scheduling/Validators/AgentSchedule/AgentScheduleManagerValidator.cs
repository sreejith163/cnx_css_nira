using Css.Api.Scheduling.Models.Domain;
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
    public class AgentScheduleManagerValidator<T> : PropertyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagerValidator{T}" /> class.
        /// </summary>
        public AgentScheduleManagerValidator()
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
            var item = context.PropertyValue as AgentScheduleManagerChart;

            if (item.Date == null)
            {
                validationFailures.Add(new ValidationFailure("Agent Scheduling Manager", "Date should not be empty"));
                return validationFailures;
            }
            else if (item.SchedulingCodeId == 0)
            {
                validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "SchedulingCodeId should not be empty"));
                return validationFailures;
            }
            else if (string.IsNullOrEmpty(item.StartTime))
            {
                validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "Start time is required"));
                return validationFailures;
            }
            else if (string.IsNullOrEmpty(item.EndTime))
            {
                validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "End time is required"));
                return validationFailures;
            }
            else
            {
                var startTimeHourString = item.StartTime.Split(":")[0];
                if (startTimeHourString.Length != 2)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "Start time hour should be 2 digits (For eg:  hh:mm am)"));
                    return validationFailures;
                }

                var startTimeMinuteString = item.StartTime.Split(":")[1]?.Split(" ")[0];
                if (startTimeMinuteString.Length != 2)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "Start time minute should be 2 digits (For eg:  hh:mm am)"));
                    return validationFailures;
                }

                var startTimeMeridiem = item.StartTime.Split(" ")[1]?.ToLower();
                if (startTimeMeridiem.Length != 2)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "Start time meridiem should be 'am / pm'  (For eg:  hh:mm am)"));
                    return validationFailures;
                }

                var startTimeHour = Convert.ToInt32(startTimeHourString);
                var startTimeMinute = Convert.ToInt32(startTimeMinuteString);

                if (startTimeMinute % 5 != 0)
                {
                    validationFailures.Add(new ValidationFailure("OperationHour", "Start time should be an interval of 5 mins"));
                    return validationFailures;
                }

                var endTimeHourString = item.EndTime.Split(":")[0];
                if (endTimeHourString.Length != 2)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "End time hour should be 2 digits (For eg:  hh:mm am)"));
                    return validationFailures;
                }

                var endTimeMinuteString = item.EndTime.Split(":")[1].Split(" ")[0];
                if (endTimeMinuteString.Length != 2)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "End time minute should be 2 digits (For eg:  hh:mm am)"));
                    return validationFailures;
                }

                var endTimeMeridiem = item.EndTime.Split(" ")[1];
                if (startTimeMeridiem.Length != 2)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "End time meridiem should be 'am / pm' (For eg:  hh:mm am)"));
                    return validationFailures;
                }

                var endTimeHour = Convert.ToInt32(endTimeHourString);
                var endTimeMinute = Convert.ToInt32(endTimeMinuteString);

                if (endTimeMinute % 5 != 0)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "End time should be an interval of 15 mins"));
                    return validationFailures;
                }

                startTimeHour = startTimeMeridiem == "am" ? startTimeHour : startTimeHour + 12;
                endTimeHour = endTimeMeridiem == "am" ? endTimeHour : endTimeHour + 12;

                DateTime startTimeDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startTimeHour, startTimeMinute, 0);
                DateTime endTimeDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endTimeHour, endTimeMinute, 0);

                if (startTimeDateTime >= endTimeDateTime)
                {
                    validationFailures.Add(new ValidationFailure("Agent Scheduling Chart", "Start time and End time range is not valid"));
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
