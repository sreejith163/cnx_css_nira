using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Sources
{
    /// <summary>
    /// The estart import source class
    /// </summary>
    public class EStartImportSource : ISource
    {
        #region Private Properties

        /// <summary>
        /// The FTP helper service
        /// </summary>
        private readonly IFTPService _ftp;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the source
        /// </summary>
        public string Name => "EStartImp";
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="ftp"></param>
        public EStartImportSource(IFTPService ftp)
        {
            _ftp = ftp;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to pull the data from the source
        /// </summary>
        /// <returns>A list of instances of DataFeed</returns>
        public async Task<List<DataFeed>> Pull()
        {
            return await Task.FromResult(_ftp.Read());
        }
        #endregion
    }
}
