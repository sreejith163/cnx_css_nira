using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Job.Repository.Interfaces
{
    /// <summary>
    /// An interface for the timezone repository
    /// </summary>
    public interface ITimezoneRepository
    {
        /// <summary>
        /// A method that fetches all the timezones existing in the system
        /// </summary>
        /// <returns>A list of instances of Timezone</returns>
        Task<List<Timezone>> GetTimezones();
    }
}
