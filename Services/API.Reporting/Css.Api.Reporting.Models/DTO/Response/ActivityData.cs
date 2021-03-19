using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Response
{
    public class ActivityData
    {
        /// <summary>
        /// The dataset of the activity
        /// </summary>
        public string DataSet { get; set; }

        /// <summary>
        /// The size of the dataset
        /// </summary>
        public int Bytes { get; set; }

        /// <summary>
        /// The metadata information 
        /// </summary>
        public string Metadata { get; set; }
    }
}
