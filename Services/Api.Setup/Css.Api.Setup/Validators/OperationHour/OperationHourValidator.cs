using Css.Api.Setup.Models.DTO.Request.OperationHour;
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
            }

            foreach (var item in list)
            {
                if (item.OperationHourOpenTypeId == 2)
                {
                    if (string.IsNullOrEmpty(item.From) && string.IsNullOrEmpty(item.To))
                    {
                        validationFailures.Add(new ValidationFailure("Operation Hour", "Operation FROM time and TO time is required"));
                    }
                    else if (string.IsNullOrEmpty(item.From))
                    {
                        validationFailures.Add(new ValidationFailure("Operation Hour", "Operation FROM time is required"));
                    }
                    else if (string.IsNullOrEmpty(item.To))
                    {
                        validationFailures.Add(new ValidationFailure("Operation Hour", "Operation TO time is required"));
                    }
                    else
                    {
                        var fromTime = item.From;
                        var fromHour = Convert.ToInt32(fromTime.Split(":")[0]);
                        var fromMinute = Convert.ToInt32(fromTime.Split(":")[1].Split(" ")[0]);
                        var fromMeridiem = fromTime.Split(" ")[1]?.ToLower();

                        if (fromMinute != 15 && fromMinute != 30 && fromMinute != 45 && fromMinute != 0)
                        {
                            validationFailures.Add(new ValidationFailure("OperationHour", "The FROM time should be an interval of 15 mins"));
                        }

                        var toTime = item.To;
                        var toHour = Convert.ToInt32(toTime.Split(":")[0]);
                        var toMinute = Convert.ToInt32(toTime.Split(":")[1].Split(" ")[0]);
                        var toMeridiem = toTime.Split(" ")[1];

                        if (toMinute != 15 && toMinute != 30 && toMinute != 45 && toMinute != 0)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "The TO time should be an interval of 15 mins"));
                        }

                        DateTime fromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, fromHour, fromMinute, fromMeridiem == "am" ? 0 : 1);
                        DateTime toDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, toHour, toMinute, toMeridiem == "am" ? 0 : 1);

                        if (fromDateTime > toDateTime)
                        {
                            validationFailures.Add(new ValidationFailure("Operation Hour", "The time range is not valid"));
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
