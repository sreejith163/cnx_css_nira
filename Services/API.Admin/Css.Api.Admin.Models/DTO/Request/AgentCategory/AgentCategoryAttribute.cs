namespace Css.Api.Admin.Models.DTO.Request.AgentCategory
{
    public class AgentCategoryAttribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the type of the data.</summary>
        /// <value>The type of the data.</value>
        public int DataTypeId { get; set; }

        /// <summary>Gets or sets the data type minimum value.</summary>
        /// <value>The data type minimum value.</value>
        public string DataTypeMinValue { get; set; }

        /// <summary>Gets or sets the data type maximum value.</summary>
        /// <value>The data type maximum value.</value>
        public string DataTypeMaxValue { get; set; }

    }
}
