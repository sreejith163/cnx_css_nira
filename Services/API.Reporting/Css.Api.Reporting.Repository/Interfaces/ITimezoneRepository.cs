using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for timezone repository
    /// </summary>
    public interface ITimezoneRepository
    {
        /// <summary>
        /// The method to fetch the timezone matching the input timezone id
        /// </summary>
        /// <param name="timezoneId"></param>
        /// <returns>The instance of Timezone</returns>
        Task<Timezone> GetTimezone(int timezoneId);

        /// <summary>
        /// The method to fetch all the timezone matching the input timezone ids
        /// </summary>
        /// <param name="timezoneIds"></param>
        /// <returns>The list of instances of Timezone</returns>
        Task<List<Timezone>> GetTimezones(List<int> timezoneIds);
    }
}
