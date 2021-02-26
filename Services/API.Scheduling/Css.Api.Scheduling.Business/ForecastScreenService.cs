using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.ForecastScreen;
using Css.Api.Scheduling.Models.DTO.Response.ForecastScreen;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Css.Api.Core.Models.Domain;
using System.Net;
using AutoMapper;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Scheduling.Models.Domain;

namespace Css.Api.Scheduling.Business
{
    public class ForecastScreenService : IForecastScreenService
    {
      
        private readonly IForecastScreenRepository _forecastScreenRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public ForecastScreenService(
           IHttpContextAccessor httpContextAccessor,
           IForecastScreenRepository forecastScreenRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _forecastScreenRepository = forecastScreenRepository;
            _mapper = mapper;
            _uow = uow;

        }
        public async Task<CSSResponse> GetForecastScreen(ForeCastScreenQueryParameter forecastScreenQueryparameter)
        {
            var forecastScreens = await _forecastScreenRepository.GetForecastScreens(forecastScreenQueryparameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(forecastScreens));

            return new CSSResponse(forecastScreens, HttpStatusCode.OK);
        }



        public async Task<CSSResponse> GetForecastScreenBySkillGroupId(CreateForecastData createForecastData)
        {
            var timezone = await _forecastScreenRepository.GetForecastScreenBySkillGroupId(createForecastData);
            if (timezone == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(timezone, HttpStatusCode.OK);
        }

        public async Task<CSSResponse> GetForecastData(CreateForecastData createForecastData)
        {
            var timezone = await _forecastScreenRepository.GetForecastScreenBySkillGroupId(createForecastData);
            if (timezone == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(timezone, HttpStatusCode.OK);
        }
  
        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="forecastData">The agent admin details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateForecastData(CreateForecastData forecastData)
        {
        

           

            long forecastIdDetails = forecastData.ForecastId;
            
            var forecastIDConflict = await _forecastScreenRepository.GetForecastDataID(forecastIdDetails);


            if (forecastIDConflict != null)
            {
                return new CSSResponse($"Forecast data already exist.", HttpStatusCode.Conflict);
            }

            var forecastDataRequest = _mapper.Map<ForecastScreen>(forecastData);
            _forecastScreenRepository.CreateForecastData(forecastDataRequest);
            await _uow.Commit();
            return new CSSResponse(forecastDataRequest, HttpStatusCode.Created);
        }
        public async Task<CSSResponse> UpdateForecastData(UpdateForecastData updateForecastDetails, long forecastID)
        {
            var forecast = await _forecastScreenRepository.GetForecastDataID(forecastID);
            if (forecast == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            

           

            if (forecast.ForecastId != forecastID)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }
         



            var forecastRequest = _mapper.Map(updateForecastDetails, forecast);
            _forecastScreenRepository.UpdateForecastData(forecastRequest);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

    }
}
