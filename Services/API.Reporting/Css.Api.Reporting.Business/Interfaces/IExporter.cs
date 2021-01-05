using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// An interface which is to be implemented for all exports
    /// </summary>
    public interface IExporter
    {
        /// <summary>
        /// A unique name given to each class implementing the interface. This will be mapped to the 'service' value in mapper json for the corresponding key 
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        ///  The processing logic is to be implemented here
        /// </summary>
        /// <returns>An instance of ExportResponse</returns>
        public ExportResponse Process();
    }
}
