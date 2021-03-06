﻿using Css.Api.Setup.Models.DTO.Request.OperationHour;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;

namespace Css.Api.Setup.Validators.OperationHour
{
    /// <summary>
    /// Custom validator for operation hours time range
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FluentValidation.Validators.PropertyValidator" />
    public class OperationHourValidator<T> : PropertyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationHourValidator{T}" /> class.
        /// </summary>
        public OperationHourValidator()
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
            var list = context.PropertyValue as IList<OperationHourAttribute>;

            if (list.Count != 7)
            {
                validationFailures.Add(new ValidationFailure("Operation Hour", "The operating hours should be defined for a full week"));
                return validationFailures;
            }

            foreach (var item in list)
            {
                if (item.OperationHourOpenTypeId == 2)
                {
                    if (string.IsNullOrEmpty(item.From) && string.IsNullOrEmpty(item.To))
                    {
                        validationFailures.Add(new ValidationFailure("Operation Hour", "FROM time and TO time is required"));
                        return validationFailures;
                    }
                    else if (string.IsNullOrEmpty(item.From))
                    {
                        validationFailures.Add(new ValidationFailure("Operation Hour", "FROM time is required"));
                        return validationFailures;
                    }
                    else if (string.IsNullOrEmpty(item.To))
                    {
                        validationFailures.Add(new ValidationFailure("Operation Hour", "TO time is required"));
                        return validationFailures;
                    }
                    else
                    {
                        var fromHourString = item.From.Split(":")[0];
                        if (fromHourString.Length != 2)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "FROM time hour should be 2 digits"));
                            return validationFailures;
                        }

                        var fromMinuteString = item.From.Split(":")[1]?.Split(" ")[0];
                        if (fromMinuteString.Length != 2)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "FROM time minute should be 2 digits"));
                            return validationFailures;
                        }

                        var fromMeridiem = item.From.Split(" ")[1]?.ToLower();
                        if (fromMeridiem.Length != 2)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "FROM time meridiem should be 'am / pm'"));
                            return validationFailures;
                        }

                        var fromHour = Convert.ToInt32(fromHourString);
                        var fromMinute = Convert.ToInt32(fromMinuteString);

                        if (fromMinute != 15 && fromMinute != 30 && fromMinute != 45 && fromMinute != 0)
                        {
                            validationFailures.Add(new ValidationFailure("OperationHour", "FROM time should be an interval of 15 mins"));
                            return validationFailures;
                        }

                        var toHourString = item.To.Split(":")[0];
                        if (toHourString.Length != 2)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "TO time hour should be 2 digits"));
                            return validationFailures;
                        }

                        var toMinuteString = item.To.Split(":")[1].Split(" ")[0];
                        if (toMinuteString.Length != 2)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "TO time minute should be 2 digits"));
                            return validationFailures;
                        }

                        var toMeridiem = item.To.Split(" ")[1];
                        if (fromMeridiem.Length != 2)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "TO time meridiem should be 'am / pm'"));
                            return validationFailures;
                        }

                        var toHour = Convert.ToInt32(toHourString);
                        var toMinute = Convert.ToInt32(toMinuteString);

                        if (toMinute != 15 && toMinute != 30 && toMinute != 45 && toMinute != 0)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "TO time should be an interval of 15 mins"));
                            return validationFailures;
                        }

                        fromHour = fromMeridiem == "am" ? fromHour : fromHour + 12;
                        toHour = toMeridiem == "am" ? toHour : toHour + 12;

                        DateTime fromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, fromHour, fromMinute, 0);
                        DateTime toDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, toHour, toMinute, 0);

                        if (fromDateTime > toDateTime)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "FROM and TO time range is not valid"));
                            return validationFailures;
                        }
                    }
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
