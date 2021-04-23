using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Mappers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The configuration service
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        #region Public Properties
        /// <summary>
        /// The Config
        /// </summary>
        public ConfigSettings Settings { get; }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigurationService(IConfiguration configuration)
        {
            Settings = new ConfigSettings();
            configuration.Bind("AgentCategoryFields", Settings.AgentCategoryFields);
            configuration.Bind("MUs", Settings.MUs);
            configuration.Bind("Batch", Settings.Batch);
        }
        #endregion
    }
}
