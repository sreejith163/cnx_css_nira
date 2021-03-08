using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ForecastScreen;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface IForecastScreenRepository
    {
        Task<PagedList<Entity>> GetForecastScreens(ForeCastScreenQueryParameter forecastScreenQueryparameter);

        Task<ForecastScreen> GetForecastData(ForecastIdDetails forecastIdDetails);

        Task<ForecastScreen> GetForecastDataID(long forecastID);
        Task<ForecastScreen> GetForecastScreenBySkillGroupId(ForecastIdDetails forecastIdDetails);


        //void ImportAgentScheduleChart(ImportForecast importForecast);

        void CreateForecastData(ForecastScreen forecastDataRequest);

        void CreateMultipleForecastData(List<ForecastScreen> forecastDataRequest);

        /// <summary>
        /// Updates the ForecastData.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        void UpdateForecastData(ForecastScreen forecastDataRequest);

    }
}
