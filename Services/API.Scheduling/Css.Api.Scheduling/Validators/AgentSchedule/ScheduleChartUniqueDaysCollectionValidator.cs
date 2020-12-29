using Css.Api.Scheduling.Models.Domain;
using FluentValidation.Validators;
using System.Collections.Generic;
using System.Linq;

namespace Css.Api.Scheduling.Validators.AgentSchedule
{
    /// <summary>
    /// Validator for checking the uniqueness of schedulign chart
    /// </summary>
    /// <seealso cref="FluentValidation.Validators.PropertyValidator" />
    public class ScheduleChartUniqueDaysCollectionValidator : PropertyValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleChartUniqueDaysCollectionValidator"/> class.
        /// </summary>
        public ScheduleChartUniqueDaysCollectionValidator()
        : base("The following day already exist: {symbols}")
        {
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
            var collection = context.PropertyValue as List<AgentScheduleChart>;
            if (collection == null)
            {
                return true;
            }

            var nonUniqueKeys = collection.GroupBy(x => x.Day).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            if (nonUniqueKeys.Count > 0)
            {
                string failedItems = string.Join(", ", nonUniqueKeys.ToArray());
                context.MessageFormatter.AppendArgument("symbols", failedItems);
                return false;
            }

            return true;
        }
    }
}
