using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request;
using Css.Api.Reporting.Models.DTO.Request.CNX1;
using Css.Api.Reporting.Models.DTO.Request.EStart;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

        /// <summary>
        /// A generic method to return the filter params for the current POST/PUT request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The instance of T</returns>
        public T GetFilterParams<T>()
            where T : class
        {
            T dto;
            if (typeof(T).Equals(typeof(EStartFilter)))
            {
                dto = JsonConvert.DeserializeObject<T>(_context.RequestBody);
            }
            else
            {
                throw new NotImplementedException();
            }
            return dto;
        }

        /// <summary>
        /// A generic method to return the query params for the current GET request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The instance of T</returns>
        public T GetQueryParams<T>()
            where T : class
        {
            T dto;
            if (typeof(T).Equals(typeof(CNX1Filter)))
            {
                dto = JsonConvert.DeserializeObject<T>(_context.RequestQueryParams);
            }
            else
            {
                throw new NotImplementedException();
            }
            return dto;
        }
        #endregion
    }
}
