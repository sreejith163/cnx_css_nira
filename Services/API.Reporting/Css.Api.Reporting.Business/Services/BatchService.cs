using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The batch service
    /// </summary>
    public class BatchService : IBatchService
    {
        #region Private Properties

        /// <summary>
        /// The batch settings
        /// </summary>
        private readonly BatchSettings _settings;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="configuration"></param>
        public BatchService(IConfigurationService configuration)
        {
            _settings = configuration.Settings.Batch;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// A method to generate batches for input source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<Batch<T>> GenerateBatches<T>(List<T> source)
            where T : class
        {
            var batchId = DateTime.UtcNow.ToString("yyyyMMddHHmm");
            return source.Select((item, inx) => new { item, inx })
                    .GroupBy(x => x.inx / _settings.MaxSize)
                    .Select(g => new Batch<T> { Items = g.Select(x => x.item).ToList(), BatchId = string.Join("B", batchId, (g.Key + 1) > 10 ? (g.Key + 1).ToString() : "0" + (g.Key + 1)) }).ToList();
        }
        #endregion
    }
}
