using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView
{
    public class AgentCategoryGenericValidator
    {
        public int AgentCategoryId { get; set; }
        public string Name { get; set; }

        public AgentCategoryType AgentCategoryType { get; set; }

        /// <summary>
        /// Gets or sets the data type minimum value.
        /// </summary>
        public string DataTypeMinValue { get; set; }

        /// <summary>
        /// Gets or sets the data type maximum value.
        /// </summary>
        public string DataTypeMaxValue { get; set; }

        //public string DataTypeLabel { get; set; }


    }
}
