using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.SchedulingCode;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>The bus</summary>
        private readonly IBusService _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public SchedulingCodeService(IRepositoryWrapper repository,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IBusService bus)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _bus = bus;
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
            var schedulingCodes = await _repository.SchedulingCodes.GetSchedulingCodesByDescriptionAndIconOrRefId(new SchedulingCodeAttributes
            {
                IconId = schedulingCodeDetails.IconId,
                Description = schedulingCodeDetails.Description,
                RefId = schedulingCodeDetails.RefId
            });

            if (schedulingCodes?.Count > 0 && schedulingCodes[0].RefId != null && schedulingCodes[0].RefId == schedulingCodeDetails.RefId)
            {
                return new CSSResponse($"SchedulingCode with id '{schedulingCodeDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (schedulingCodes?.Count > 0 && schedulingCodes[0].Description == schedulingCodeDetails.Description)
            {
                return new CSSResponse($"SchedulingCode with description '{schedulingCodeDetails.Description}' already exists.", HttpStatusCode.Conflict);
            }
            else if (schedulingCodes?.Count > 0 && schedulingCodes[0].IconId == schedulingCodeDetails.IconId)
            {
                return new CSSResponse($"SchedulingCode with icon '{schedulingCodes[0].Icon}' already exists.", HttpStatusCode.Conflict);
            }

            var schedulingCodeRequest = _mapper.Map<SchedulingCode>(schedulingCodeDetails);

            _repository.SchedulingCodes.CreateSchedulingCode(schedulingCodeRequest);

            await _repository.SaveAsync();

            await _bus.SendCommand<CreateSchedulingCodeCommand>(MassTransitConstants.SchedulingCodeCreateCommandRouteKey,
               new
               {
                   schedulingCodeRequest.Id,
                   Name = schedulingCodeRequest.Description,
                   schedulingCodeRequest.PriorityNumber,
                   schedulingCodeRequest.TimeOffCode,
                   schedulingCodeRequest.IconId,
                   SchedulingTypeCode = JsonConvert.SerializeObject(schedulingCodeDetails.SchedulingTypeCode),
                   schedulingCodeRequest.ModifiedDate
               });

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

            var schedulingCodes = await _repository.SchedulingCodes.GetSchedulingCodesByDescriptionAndIconOrRefId(new SchedulingCodeAttributes
            {
                IconId = schedulingCodeDetails.IconId,
                Description = schedulingCodeDetails.Description,
                RefId = schedulingCodeDetails.RefId
            });
            var result = schedulingCodes.Find(x => x.Id != schedulingCodeIdDetails.SchedulingCodeId);
            if (result != null && result.RefId != null && result.RefId == schedulingCodeDetails.RefId)
            {
                return new CSSResponse($"SchedulingCode with id '{schedulingCodeDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (result != null && result.Description == schedulingCodeDetails.Description)
            {
                return new CSSResponse($"SchedulingCode with description '{schedulingCodeDetails.Description}' already exists.", HttpStatusCode.Conflict);
            }
            else if (result != null && result.IconId == schedulingCodeDetails.IconId)
            {
                return new CSSResponse($"SchedulingCode with icon '{schedulingCodes[0].Icon}' already exists.", HttpStatusCode.Conflict);
            }

            SchedulingCode schedulingCodeDetailsPreUpdate = null;
            if (!schedulingCodeDetails.IsUpdateRevert)
            {
                List<SchedulingTypeCode> schedulingTypeCodePreUpdated = new List<SchedulingTypeCode>(schedulingCode.SchedulingTypeCode);
                schedulingCodeDetailsPreUpdate = new SchedulingCode
                {
                    RefId = schedulingCode.RefId,
                    Description = schedulingCode.Description,
                    PriorityNumber = schedulingCode.PriorityNumber,
                    TimeOffCode = schedulingCode.TimeOffCode,
                    IconId = schedulingCode.IconId,
                    SchedulingTypeCode = schedulingTypeCodePreUpdated,
                    ModifiedBy = schedulingCode.ModifiedBy,
                    IsDeleted = schedulingCode.IsDeleted,
                    ModifiedDate = schedulingCode.ModifiedDate
                };
            }

            _repository.SchedulingTypeCodes.RemoveSchedulingTypeCodes(schedulingCode.SchedulingTypeCode.ToList());

            var schedulingCodeRequest = _mapper.Map(schedulingCodeDetails, schedulingCode);

            if (schedulingCodeDetails.IsUpdateRevert)
            {
                schedulingCodeRequest.ModifiedDate = schedulingCodeDetails.ModifiedDate;
            }

            _repository.SchedulingCodes.UpdateSchedulingCode(schedulingCodeRequest);

            await _repository.SaveAsync();

            if (!schedulingCodeDetails.IsUpdateRevert)
            {
                UpdateSchedulingCode schedulingCodePreUpdate = null;
                var schedulingCodePreRequest = _mapper.Map(schedulingCodeDetailsPreUpdate, schedulingCodePreUpdate);

                await _bus.SendCommand<UpdateSchedulingCodeCommand>(
                    MassTransitConstants.SchedulingCodeUpdateCommandRouteKey,
                    new
                    {
                        schedulingCodeRequest.Id,
                        NameOldValue = schedulingCodeDetailsPreUpdate.Description,
                        PriorityNumberOldValue = schedulingCodeDetailsPreUpdate.PriorityNumber,
                        TimeOffCodeOldValue = schedulingCodeDetailsPreUpdate.TimeOffCode,
                        IconIdOldValue = schedulingCodeDetailsPreUpdate.IconId,
                        SchedulingTypeCodeOldValue = JsonConvert.SerializeObject(schedulingCodePreRequest.SchedulingTypeCode),
                        ModifiedByOldValue = schedulingCodeDetailsPreUpdate.ModifiedBy,
                        ModifiedDateOldValue = schedulingCodeDetailsPreUpdate.ModifiedDate,
                        IsDeletedOldValue = schedulingCodeDetailsPreUpdate.IsDeleted,
                        NameNewValue = schedulingCodeRequest.Description,
                        IsDeletedNewValue = schedulingCodeRequest.IsDeleted
                    });
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }


        public async Task<CSSResponse> RevertSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails, UpdateSchedulingCode schedulingCodeDetails)
        {
            var schedulingCode = await _repository.SchedulingCodes.GetAllSchedulingCode(schedulingCodeIdDetails);
            if (schedulingCode == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var schedulingCodes = await _repository.SchedulingCodes.GetAllSchedulingCodesByDescription(new SchedulingCodeNameDetails { Name = schedulingCodeDetails.Description });
            if (schedulingCodes?.Count > 0 && schedulingCodes.IndexOf(schedulingCodeIdDetails.SchedulingCodeId) == -1)
            {
                return new CSSResponse($"SchedulingCode with description '{schedulingCodeDetails.Description}' already exists.", HttpStatusCode.Conflict);
            }

            _repository.SchedulingTypeCodes.RemoveSchedulingTypeCodes(schedulingCode.SchedulingTypeCode.ToList());

            var schedulingCodeRequest = _mapper.Map(schedulingCodeDetails, schedulingCode);


            schedulingCodeRequest.ModifiedDate = schedulingCodeDetails.ModifiedDate;


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

            SchedulingCode schedulingCodeDetailsPreUpdate = null;

            schedulingCodeDetailsPreUpdate = new SchedulingCode
            {
                Description = schedulingCode.Description,
                PriorityNumber = schedulingCode.PriorityNumber,
                TimeOffCode = schedulingCode.TimeOffCode,
                IconId = schedulingCode.IconId,
                SchedulingTypeCode = schedulingCode.SchedulingTypeCode,
                ModifiedBy = schedulingCode.ModifiedBy,
                IsDeleted = schedulingCode.IsDeleted,
                ModifiedDate = schedulingCode.ModifiedDate
            };

            schedulingCode.IsDeleted = true;

            _repository.SchedulingCodes.UpdateSchedulingCode(schedulingCode);
            await _repository.SaveAsync();

            UpdateSchedulingCode schedulingCodePreUpdate = null;
            var schedulingCodePreRequest = _mapper.Map(schedulingCodeDetailsPreUpdate, schedulingCodePreUpdate);

            await _bus.SendCommand<DeleteSchedulingCodeCommand>(
               MassTransitConstants.SchedulingCodeDeleteCommandRouteKey,
               new
               {
                   schedulingCode.Id,
                   Name = schedulingCodeDetailsPreUpdate.Description,
                   schedulingCodeDetailsPreUpdate.PriorityNumber,
                   schedulingCodeDetailsPreUpdate.TimeOffCode,
                   schedulingCodeDetailsPreUpdate.IconId,
                   SchedulingTypeCode = JsonConvert.SerializeObject(schedulingCodePreRequest.SchedulingTypeCode),
                   ModifiedByOldValue = schedulingCodeDetailsPreUpdate.ModifiedBy,
                   IsDeletedOldValue = schedulingCodeDetailsPreUpdate.IsDeleted,
                   ModifiedDateOldValue = schedulingCodeDetailsPreUpdate.ModifiedDate,
                   IsDeletedNewValue = schedulingCode.IsDeleted
               });

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
