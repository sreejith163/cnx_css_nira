using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The helper service to access the context mapping details
    /// </summary>
    public class MapperService : IMapperService
    {
        #region Private Properties

        /// <summary>
        /// The mapping context of the current request
        /// </summary>
        private readonly MappingContext _context;

        /// <summary>
        /// The FTP service
        /// </summary>
        private readonly IFTPService _ftp;
        #endregion

        #region Public Properties
        /// <summary>
        /// The mapper context object of the current request
        /// </summary>
        public MappingContext Context => _context;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="ftp"></param>
        public MapperService(IHttpContextAccessor httpContextAccessor, IFTPService ftp)
        {
            _context = (MappingContext)httpContextAccessor.HttpContext.Items["Mappers"];
            _ftp = ftp;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to initialize the FTP service for the source/target
        /// </summary>
        /// <typeparam name="T">ISource/ITarget</typeparam>
        public void InitializeFTP<T>()
            where T : class
        {
            Dictionary<string, string> options = new Dictionary<string, string>() {
                { "FTPServer" , "" },
                { "FTPInbox" , "" },
                { "FTPOutbox" , "" }
            };

            var key = Context.Key.Split('-').First();

            if (typeof(T).GetTypeInfo().IsAssignableFrom(typeof(ISource).Ge‌​tTypeInfo()))
            {
                options["FTPServer"] = _context.SourceOptions["FTPServer"];
                options["FTPInbox"] = string.Format(_context.SourceOptions["FTPInbox"], key);
            }
            else if (typeof(T).GetTypeInfo().IsAssignableFrom(typeof(ITarget).Ge‌​tTypeInfo()))
            {
                options["FTPServer"] = _context.TargetOptions["FTPServer"];
                options["FTPOutbox"] = string.Format(_context.TargetOptions["FTPOutbox"], key);
            }
            else
            {
                throw new NotImplementedException();
            }

            _ftp.Set(options);
        }
        #endregion
    }
}
