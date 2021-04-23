using Css.Api.Reporting.Models.DTO.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface for configuration service
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// The Config
        /// </summary>
        ConfigSettings Settings { get; }
    }
}
