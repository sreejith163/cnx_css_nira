using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Interfaces
{
    /// <summary>
    /// The interface for batch service
    /// </summary>
    public interface IBatchService
    {
        /// <summary>
        /// A method to generate batches for input source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        List<Batch<T>> GenerateBatches<T>(List<T> source)
            where T : class;
    }
}
