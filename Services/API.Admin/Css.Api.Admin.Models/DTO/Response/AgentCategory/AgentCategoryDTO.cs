using System;

namespace Css.Api.Admin.Models.DTO.Response.AgentCategory
{
    public class AgentCategoryDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the type of the data.</summary>
        /// <value>The type of the data.</value>
        public int DataTypeId { get; set; }

        /// <summary>Gets or sets the data type label.</summary>
        /// <value>The data type label.</value>
        public string DataTypeLabel { get; set; }

        /// <summary>Gets or sets the data type minimum value.</summary>
        /// <value>The data type minimum value.</value>
        public string DataTypeMinValue { get; set; }

        /// <summary>Gets or sets the data type maximum value.</summary>
        /// <value>The data type maximum value.</value>
        public string DataTypeMaxValue { get; set; }

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


