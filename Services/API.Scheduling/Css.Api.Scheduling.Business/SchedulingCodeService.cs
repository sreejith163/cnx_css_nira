using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    public class SchedulingCodeService : ISchedulingCodeService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public SchedulingCodeService(IRepositoryWrapper repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeParameters">The scheduling code parameters.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetSchedulingCodes(SchedulingCodeQueryParameters schedulingCodeParameters)
        {
            var schedulingCodes = await _repository.SchedulingCodes.GetSchedulingCodes(schedulingCodeParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(schedulingCodes));

            return new CSSResponse(schedulingCodes, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails)
        {
            var schedulingCode = await _repository.SchedulingCodes.GetSchedulingCode(schedulingCodeIdDetails);
            if (schedulingCode == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedSchedulingCode = _mapper.Map<SchedulingCodeDTO>(schedulingCode);
            return new CSSResponse(mappedSchedulingCode, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeDetails">The scheduling code details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateSchedulingCode(CreateSchedulingCode schedulingCodeDetails)
        {
            var schedulingCodes = await _repository.SchedulingCodes.GetSchedulingCodesByDescription(new SchedulingCodeNameDetails { Name = schedulingCodeDetails.Description });
            if (schedulingCodes?.Count > 0)
            {
                return new CSSResponse($"SchedulingCode with description '{schedulingCodeDetails.Description}' already exists.", HttpStatusCode.Conflict);
            }

            var schedulingCodeRequest = _mapper.Map<SchedulingCode>(schedulingCodeDetails);

            _repository.SchedulingCodes.CreateSchedulingCode(schedulingCodeRequest);

            await _repository.SaveAsync();

            return new CSSResponse(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <param name="schedulingCodeDetails">The scheduling code details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails, UpdateSchedulingCode schedulingCodeDetails)
        {
            var schedulingCode = await _repository.SchedulingCodes.GetSchedulingCode(schedulingCodeIdDetails);
            if (schedulingCode == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var schedulingCodes = await _repository.SchedulingCodes.GetSchedulingCodesByDescription(new SchedulingCodeNameDetails { Name = schedulingCodeDetails.Description });
            if (schedulingCodes?.Count > 0 && schedulingCodes.IndexOf(schedulingCodeIdDetails.SchedulingCodeId) == -1)
            {
                return new CSSResponse($"SchedulingCode with description '{schedulingCodeDetails.Description}' already exists.", HttpStatusCode.Conflict);
            }

            _repository.SchedulingTypeCodes.RemoveSchedulingTypeCodes(schedulingCode.SchedulingTypeCode.ToList());

            var schedulingCodeRequest = _mapper.Map(schedulingCodeDetails, schedulingCode);

            _repository.SchedulingCodes.UpdateSchedulingCode(schedulingCodeRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails)
        {
            var schedulingCode = await _repository.SchedulingCodes.GetSchedulingCode(schedulingCodeIdDetails);
            if (schedulingCode == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            schedulingCode.IsDeleted = true;

            _repository.SchedulingCodes.UpdateSchedulingCode(schedulingCode);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
