using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Request;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Css.Api.Reporting.Business.Factories
{
    /// <summary>
    /// The factory to resolve all the services
    /// </summary>
    public class ServiceFactory : IServiceFactory
    {
        #region Private Properties 
        
        /// <summary>
        /// The mapping context
        /// </summary>
        private readonly MappingContext _context;

        /// <summary>
        /// The mapper service
        /// </summary>
        private readonly IMapperService _mapper;

        /// <summary>
        /// An Enumerable of all available ISource
        /// </summary>
        private readonly IEnumerable<ISource> _sources;

        /// <summary>
        /// An Enumerable of all available ITargets
        /// </summary>
        private readonly IEnumerable<ITarget> _targets;
        #endregion

        #region Constructor
        /// <summary>
        ///  Constructor to initialize all properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="sources"></param>
        /// <param name="targets"></param>
        public ServiceFactory(IMapperService mapper, IEnumerable<ISource> sources, IEnumerable<ITarget> targets)
        {
            _mapper = mapper;
            _context = mapper.Context;
            _sources = sources;
            _targets = targets;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// A generic mapper to map a corresponding source/target for the current request
        /// </summary>
        /// <typeparam name="T">A class of ISource/ITarget</typeparam>
        /// <returns>An instance of T</returns>
        public T Map<T>()
            where T : class
        {
            T service;
            if(typeof(T).GetTypeInfo().IsAssignableFrom(typeof(ISource).Ge‌​tTypeInfo()))
            {
                service = (T) _sources.FirstOrDefault(x => string.Equals(x.Name, _context.Source, StringComparison.InvariantCultureIgnoreCase));
            }
            else if(typeof(T).GetTypeInfo().IsAssignableFrom(typeof(ITarget).Ge‌​tTypeInfo()))
            {
                service = (T) _targets.FirstOrDefault(x => string.Equals(x.Name, _context.Target, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                throw new MappingException(string.Format(Messages.MappingNotFound, _context.Key));
            }

            return service;
        }

        /// <summary>
        /// The method to initialize the data options
        /// </summary>
        public void Initialize()
        {
            if(_mapper.Context.SourceType.Equals(DataOptions.FTP.GetDescription(), StringComparison.InvariantCultureIgnoreCase))
            {
                _mapper.InitializeFTP<ISource>();
            }
            else if(_mapper.Context.TargetType.Equals(DataOptions.FTP.GetDescription(), StringComparison.InvariantCultureIgnoreCase))
            {
                _mapper.InitializeFTP<ITarget>();
            }
        }
        #endregion
    }

}
