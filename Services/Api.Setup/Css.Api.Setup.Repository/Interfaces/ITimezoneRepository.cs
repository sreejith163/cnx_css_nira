using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository.Interfaces
{
    public interface ITimezoneRepository
    {
        /// <summary>Gets the timezones.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<KeyValue>> GetTimezones();
    }
}