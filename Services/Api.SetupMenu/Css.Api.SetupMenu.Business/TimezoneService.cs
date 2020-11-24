using Css.Api.Core.Models.DTO.Response;
using Css.Api.SetupMenu.Business.Interfaces;
using Css.Api.SetupMenu.Repository.Interfaces;
using System.Net;
using System.Threading.Tasks;
namespace Css.Api.SetupMenu.Business
{
    public class TimezoneService : ITimezoneService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>Initializes a new instance of the <see cref="TimezoneService" /> class.</summary>
        /// <param name="repository">The repository.</param>
        public TimezoneService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>Gets the time zones.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetTimezones()
        {
            var timeZones = await _repository.TimeZones.GetTimezones();
            return new CSSResponse(timeZones, HttpStatusCode.OK);
        }
    }
}
