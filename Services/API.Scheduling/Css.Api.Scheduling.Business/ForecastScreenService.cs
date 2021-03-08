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
using System.Globalization;

namespace Css.Api.Scheduling.Business
{
    public class ForecastScreenService : IForecastScreenService
    {
      
        private readonly IForecastScreenRepository _forecastScreenRepository;
        private readonly ISkillGroupRepository _skillGroupRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public ForecastScreenService(
           IHttpContextAccessor httpContextAccessor,
           IForecastScreenRepository forecastScreenRepository,
           ISkillGroupRepository skillGroupRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _forecastScreenRepository = forecastScreenRepository;
            _skillGroupRepository = skillGroupRepository;
            _mapper = mapper;
            _uow = uow;

        }
        public async Task<CSSResponse> GetForecastScreen(ForeCastScreenQueryParameter forecastScreenQueryparameter)
        {
            var forecastScreens = await _forecastScreenRepository.GetForecastScreens(forecastScreenQueryparameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(forecastScreens));

            return new CSSResponse(forecastScreens, HttpStatusCode.OK);
        }



        public async Task<CSSResponse> GetForecastScreenBySkillGroupId(ForecastIdDetails forecastIdDetails)
        {

            var forecast = await _forecastScreenRepository.GetForecastScreenBySkillGroupId(forecastIdDetails);
            if (forecast == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(forecast, HttpStatusCode.OK);
        }

        public async Task<CSSResponse> GetForecastData(ForecastIdDetails forecastIdDetails)
        {

            var forecast = await _forecastScreenRepository.GetForecastScreenBySkillGroupId(forecastIdDetails);
            if (forecast == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(forecast, HttpStatusCode.OK);
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
        //private void OverwriteScheduleMangerCharts(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, EmployeeIdDetails employeeIdDetails,
        //                                   ModifiedUserDetails modifiedUserDetails, List<AgentScheduleManagerChart> agentScheduleManagerCharts,
        //                                   List<AgentScheduleChart> agentScheduleCharts)
        //{
        //    if (dateFrom.HasValue && dateFrom != default(DateTimeOffset) && dateTo.HasValue && dateTo != default(DateTimeOffset))
        //    {
        //        if (agentScheduleCharts.Any())
        //        {
        //            var weekDays = agentScheduleCharts.Select(x => x.Day);
        //            var filteredScheduleManagerCharts = agentScheduleManagerCharts.FindAll(x => weekDays.Contains((int)x.Date.Date.DayOfWeek));
        //            foreach (var filteredScheduleManagerChart in filteredScheduleManagerCharts)
        //            {
        //                filteredScheduleManagerChart.Charts = new List<ScheduleChart>();
        //                _agentScheduleRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, filteredScheduleManagerChart, modifiedUserDetails);
        //            }
        //        }
        //    }
        //}

        public async Task<CSSResponse> ImportForecastData(ImportForecastDetails importForecastDetails)
        {
            List<string> forecastDataTime =  new List<string> 
            { 
                "12:00 AM", "12:30 AM", "1:00 AM", "1:30 AM", "2:00 AM","2:30 AM","3:00 AM","3:30 AM","4:00 AM","4:30 AM","5:00 AM","5:30 AM","6:00 AM","6:30 AM","7:00 AM","7:30 AM","8:00 AM","8:30 AM","9:00 AM","9:30 AM","10:00 AM","10:30 AM","11:00 AM","11:30 AM",
                "12:00 PM","12:30 PM","1:00 PM","1:30 PM","2:00 PM","2:30 PM","3:00 PM","3:30 PM","4:00 PM","4:30 PM","5:00 PM","5:30 PM","6:00 PM","6:30 PM","7:00 PM","7:30 PM","8:00 PM","8:30 PM","9:00 PM","9:30 PM","10:00 PM","10:30 PM","11:00 PM","11:30 PM",
            };

            List<string> errors = new List<string>();
            List<string> success = new List<string>();

            ForecastDataResponse forecastDataResponse = new ForecastDataResponse();

            int importCount = 0;
            int importSuccess = 0;
            var forecastScreens = importForecastDetails.ForecastScreenDataDetails;
            List<ForecastScreen> createForecastScreens = new List<ForecastScreen>();


            // check first if skillGroup exists
            var skillGroup = await _skillGroupRepository.GetSkillGroup(new SkillGroupIdDetails { SkillGroupId = importForecastDetails.SkillGroupId });
            if (skillGroup == null)
            {
                //List<string> skillGroupError = new List<string>();
                //skillGroupError.Add($"skillGroupId '{forecastScreen.SkillGroupId}' not found.");

                errors.Add($"skillGroupId '{importForecastDetails.SkillGroupId}' not found.");
            }
            else
            {
                List<ImportForecastScreenDataDetails> importForecastScreenList = new List<ImportForecastScreenDataDetails>();
                foreach (ImportForecastScreenDataDetails forecastScreen in forecastScreens)
                {
                    importCount = importCount + 1;
                    if (forecastScreen.Date == null)
                    {
                        errors.Add("Date is Empty.");
                    }
                    else
                    {
                        DateTime temp;
                        if (DateTime.TryParseExact(forecastScreen.Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        {
                            var forecastDate = DateTime.ParseExact(forecastScreen.Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                            var forecast = await _forecastScreenRepository.GetForecastScreenBySkillGroupId(new ForecastIdDetails { SkillGroupId = importForecastDetails.SkillGroupId, Date = forecastDate.ToString("yyyy-MM-dd") });

                            //var forecast = await _forecastScreenRepository.GetForecastDataID(forecastScreen.ForecastId);
                            if (forecast != null)
                            {
                                var forecastData = forecast.ForecastData;
                                List<string> updateErrors = new List<string>();
                                //foreach (var forecastDataUpdate in forecastScreen)
                                //{
                                var forecastDataUpdate = forecastScreen;
                                if (forecastData.Exists(x => x.Time == forecastDataUpdate.Time))
                                {
                                    var forecastDataDetails = forecastData.Find(x => x.Time == forecastDataUpdate.Time);
                                    forecastDataDetails.Aht = forecastDataUpdate.Aht;
                                    forecastDataDetails.ForecastedContact = forecastDataUpdate.ForecastedContact;
                                    forecastDataDetails.ForecastedReq = forecastDataUpdate.ForecastedReq;
                                }
                                else
                                {
                                    updateErrors.Add($"Invalid Time Input {forecastDataUpdate.Time}");
                                }

                                //}

                                if (updateErrors.Count > 0)
                                {
                                    errors.AddRange(updateErrors);
                                }
                                else
                                {
                                    var forecastDataMapped = _mapper.Map<List<ForecastDataAtrribute>>(forecastData);
                                    //update forecast.forecastData from forecastScreen.forecastData
                                    var updateForecastDetails = new UpdateForecastData
                                    {
                                        ForecastData = forecastDataMapped
                                    };

                                    var forecastRequest = _mapper.Map(updateForecastDetails, forecast);
                                    _forecastScreenRepository.UpdateForecastData(forecastRequest);
                                    await _uow.Commit();

                                    //List<string> successMessage = new List<string>();

                                    //successMessage.Add($"'{forecastScreen.ForecastId}' updated.");

                                    success.Add($"'1 forecast data updated.");
                                    importSuccess = importSuccess + 1;
                                }

                            }

                            // create if does not exists
                            if (forecast == null)
                            {

                                List<string> updateErrors = new List<string>();
                                var forecastPreCreate = new CreateForecastData
                                {
                                    Date = forecastDate.ToString("yyyy-MM-dd"),
                                    SkillGroupId = importForecastDetails.SkillGroupId,
                                };

                                var forecastDataPreCreate = new List<ForecastDataAtrribute>();


                                foreach (var time in forecastDataTime)
                                {

                                    var forecastDataDefault = new ForecastDataAtrribute
                                    {
                                        Time = time,
                                        Aht = 0,
                                        ForecastedContact = 0,
                                        ForecastedReq = 0,
                                        ScheduledOpen = 0,
                                    };

                                    forecastDataPreCreate.Add(forecastDataDefault);
                                }

                                forecastPreCreate.ForecastData = forecastDataPreCreate;

                                var forecastDataPreCreateRequest = _mapper.Map<ForecastScreen>(forecastPreCreate);
                                
                                var forecastData = forecastDataPreCreateRequest.ForecastData;

                                var newForecastData = forecastScreen;
                                //foreach (var newForecastData in forecastScreen.ForecastData)
                                //{
                                if (forecastData.Exists(x => x.Time == newForecastData.Time))
                                {

                                    if(importForecastScreenList.Exists(x => x.Time == newForecastData.Time && x.Date == forecastDataPreCreateRequest.Date))
                                    {
                                        updateErrors.Add($"Duplicate Entry for creating {newForecastData.Time}");
                                    }
                                    else
                                    {
                                        if(importForecastScreenList.Exists(x => x.Date == forecastDataPreCreateRequest.Date))
                                        {
                                            // check if a list with the same date exists and append the data

                                            var appendData = createForecastScreens.Find(x => x.Date == forecastDataPreCreateRequest.Date && x.SkillGroupId == forecastDataPreCreateRequest.SkillGroupId);

                                            var forecastDataDetails = appendData.ForecastData.Find(x => x.Time == newForecastData.Time);
                                            forecastDataDetails.Aht = newForecastData.Aht;
                                            forecastDataDetails.ForecastedContact = newForecastData.ForecastedContact;
                                            forecastDataDetails.ForecastedReq = newForecastData.ForecastedReq;

                                            importForecastScreenList.Add(new ImportForecastScreenDataDetails { Date = forecastDataPreCreateRequest.Date, Time = newForecastData.Time });

                                            success.Add($"{newForecastData.Time} forecast data inserted. {forecastDataPreCreateRequest.Date}");

                                            importSuccess = importSuccess + 1;
                                        }
                                        else
                                        {
                                            var forecastDataDetails = forecastData.Find(x => x.Time == newForecastData.Time);
                                            forecastDataDetails.Aht = newForecastData.Aht;
                                            forecastDataDetails.ForecastedContact = newForecastData.ForecastedContact;
                                            forecastDataDetails.ForecastedReq = newForecastData.ForecastedReq;

                                            importForecastScreenList.Add(new ImportForecastScreenDataDetails { Date = forecastDataPreCreateRequest.Date, Time = newForecastData.Time });

                                            createForecastScreens.Add(forecastDataPreCreateRequest);
                                            success.Add($"{newForecastData.Time} forecast data inserted. {forecastDataPreCreateRequest.Date}");

                                            importSuccess = importSuccess + 1;
                                        }

                                    }
                                           
                                }
                                else
                                {
                                    updateErrors.Add($"Invalid Time Input {newForecastData.Time}");
                                }
                                //}

                                if (updateErrors.Count > 0)
                                {
                                    errors.AddRange(updateErrors);
                                }
                                
                            }
                        }
                        else
                        {
                            errors.Add($"Invalid Date Format '{forecastScreen.Date}'");
                        }

                    }

                }
            }

            if (createForecastScreens.Count > 0)
            {

                _forecastScreenRepository.CreateMultipleForecastData(createForecastScreens);
                await _uow.Commit();

            }

            string importedDataCount;
            importedDataCount = $"Successfully imported {importSuccess.ToString()} out of {importCount.ToString()} Forecast Data Rows.";

            forecastDataResponse.Errors = errors;
            forecastDataResponse.Success = success;
            forecastDataResponse.ImportStatus = importedDataCount;


            return new CSSResponse(forecastDataResponse, HttpStatusCode.OK);
        }

    }
}
