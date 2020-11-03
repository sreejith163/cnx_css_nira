using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.SchedulingCode
{
    public class SchedulingCodeDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int RefId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the priority number.
        /// </summary>
        public int PriorityNumber { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public KeyValue Icon { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code types.
        /// </summary>
        public List<KeyValue> SchedulingTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
