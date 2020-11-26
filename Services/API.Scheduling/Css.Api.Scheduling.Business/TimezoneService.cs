using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using AutoMapper;
using Css.Api.Scheduling.Models.Domain;

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
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public TimezoneService(
            IHttpContextAccessor httpContextAccessor,
            ITimezoneRepository repository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetTimeZones(TimezoneQueryParameters timezoneQueryParameters)
        {
            var timeZones = await _repository.GetTimeZones(timezoneQueryParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(timeZones));

            return new CSSResponse(timeZones, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the timezone.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetTimeZone(TimezoneIdDetails timezoneIdDetails)
        {
            var timezone = await _repository.GetTimeZone(timezoneIdDetails);
            if (timezone == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(timezone, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the timezone.
        /// </summary>
        /// <param name="timezoneDetails">The timezone details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateTimeZone(CreateTimezone timezoneDetails)
        {
            var timezoneRequest = _mapper.Map<Timezone>(timezoneDetails);
            _repository.CreateTimeZone(timezoneRequest);

            await _uow.Commit();

            return new CSSResponse(new TimezoneIdDetails { TimezoneId = timezoneRequest.TimezoneId }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the timezone.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <param name="timezoneDetails">The timezone details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateTimeZone(TimezoneIdDetails timezoneIdDetails, UpdateTimezone timezoneDetails)
        {
            var timezone = await _repository.GetTimeZone(timezoneIdDetails);
            if (timezone == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var timezoneRequest = _mapper.Map(timezoneDetails, timezone);
            _repository.UpdateTimeZone(timezoneRequest);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the skill group.
        /// </summary>
        /// <param name="timezoneIdDetails">The timezone identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteTimeZone(TimezoneIdDetails timezoneIdDetails)
        {
            var timezone = await _repository.GetTimeZone(timezoneIdDetails);
            if (timezone == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            timezone.IsDeleted = true;
            _repository.UpdateTimeZone(timezone);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
