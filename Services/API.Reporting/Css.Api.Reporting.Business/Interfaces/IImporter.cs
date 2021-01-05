using Css.Api.Reporting.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// An interface which is to be implemented for all imports
    /// </summary>
    public interface IImporter
    {
        /// <summary>
        /// A unique name given to each class implementing the interface. This will be mapped to the 'service' value in mapper json for the corresponding key 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The processing logic is to be implemented here
        /// </summary>
        /// <param name="data">The input byte[] for processing</param>
        /// <returns>An instance of ImportResponse</returns>
        Task<ImportResponse> Import(byte[] data);
    }
}
