using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Sources
{
    public class EStartExportSource : ISource
    {
        #region Public Properties

        /// <summary>
        /// The name of the source
        /// </summary>
        public string Name => "EStartInput";
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to pull the data from the source
        /// </summary>
        /// <returns>A list of instances of DataFeed</returns>
        public Task<List<DataFeed>> Pull()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
