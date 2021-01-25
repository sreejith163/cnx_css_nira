using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for scheduling code repository
    /// </summary>
    public interface ISchedulingCodeRepository
    {
        /// <summary>
        /// The method to fetch all active scheduling codes
        /// </summary>
        /// <returns>The list of instances of SchedulingCode</returns>
        Task<List<SchedulingCode>> GetSchedulingCodes();
    }
}
