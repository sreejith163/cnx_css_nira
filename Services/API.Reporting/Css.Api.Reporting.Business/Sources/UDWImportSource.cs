using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Sources
{
    public class UDWImportSource : ISource
    {
        #region Private Properties

        /// <summary>
        /// The FTP service
        /// </summary>
        private readonly IFTPService _ftp;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the source
        /// </summary>
        public string Name => "UDWImp";
        #endregion

        #region Constructor

        /// <summary>
        /// The constructor to initialize the properties
        /// </summary>
        /// <param name="ftp"></param>
        public UDWImportSource(IFTPService ftp)
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
            return _ftp.Read();
        }
        #endregion
    }
}
