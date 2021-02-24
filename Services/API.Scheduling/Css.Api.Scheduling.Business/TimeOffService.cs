using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using AutoMapper;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.TimeOff;

namespace Css.Api.Scheduling.Business
{
    public class TimeOffService : ITimeOffService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly ITimeOffRepository _repository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public TimeOffService(
            IHttpContextAccessor httpContextAccessor,
            ITimeOffRepository repository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Gets the time offs.
        /// </summary>
        /// <param name="timeOffQueryparameter">The time off queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetTimeOffs(TimeOffQueryparameter timeOffQueryparameter)
        {
            var timeOffs = await _repository.GetTimeOffs(timeOffQueryparameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(timeOffs));

            return new CSSResponse(timeOffs, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the time off.
        /// </summary>
        /// <param name="timeOffIdDetails">The time off identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetTimeOff(TimeOffIdDetails timeOffIdDetails)
        {
            var timeOff = await _repository.GetTimeOff(timeOffIdDetails);
            if (timeOff == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(timeOff, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the time off.
        /// </summary>
        /// <param name="timeOffDetails">The time off details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateTimeOff(CreateTimeOff timeOffDetails)
        {
            var timeOffNameDetails = new TimeOffNameDetails { Description = timeOffDetails.Description };
            var timeOff = await _repository.GetAllTimeOffsByDescription(timeOffNameDetails);

            if (timeOff != null)
            {
                return new CSSResponse($"TimeOff with description '{timeOffDetails.Description}' already exists.", HttpStatusCode.Conflict);
            }

            var timeOffRequest = _mapper.Map<TimeOff>(timeOffDetails);
            _repository.CreateTimeOff(timeOffRequest);

            await _uow.Commit();

            return new CSSResponse(new TimeOffIdDetails { TimeOffId = timeOffRequest.Id.ToString() }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the time off.
        /// </summary>
        /// <param name="timeOffIdDetails">The time off identifier details.</param>
        /// <param name="timeOffDetails">The time off details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateTimeOff(TimeOffIdDetails timeOffIdDetails, UpdateTimeOff timeOffDetails)
        {
            var timeOff = await _repository.GetTimeOff(timeOffIdDetails);
            if (timeOff == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var timeOffRequest = _mapper.Map(timeOffDetails, timeOff);
            _repository.UpdateTimeOff(timeOffRequest);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the time off.
        /// </summary>
        /// <param name="timeOffIdDetails">The time off identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteTimeOff(TimeOffIdDetails timeOffIdDetails)
        {
            var timeOff = await _repository.GetTimeOff(timeOffIdDetails);
            if (timeOff == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            timeOff.IsDeleted = true;
            _repository.UpdateTimeOff(timeOff);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
