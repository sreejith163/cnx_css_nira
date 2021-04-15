using System;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class AgentCategoryDetails
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category value.
        /// </summary>
        public string CategoryValue { get; set; }
    }
}
