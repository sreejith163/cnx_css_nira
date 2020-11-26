using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;

namespace Css.Api.Scheduling.Business
{
    public class TimezoneService : ITimezoneService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly ITimezoneRepository _repository;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="uow">The uow.</param>
        public TimezoneService(
            IHttpContextAccessor httpContextAccessor, 
            ITimezoneRepository repository,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _uow = uow;
        }

        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetTimezones(TimezoneQueryParameters timezoneQueryParameters)
        {
            var timeZones = await _repository.GetTimezones(timezoneQueryParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(timeZones));

            return new CSSResponse(timeZones, HttpStatusCode.OK);
        }
    }
}
