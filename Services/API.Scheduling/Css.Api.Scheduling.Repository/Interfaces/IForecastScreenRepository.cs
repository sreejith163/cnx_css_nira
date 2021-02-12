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

        Task<ForecastScreen> GetForecastData(CreateForecastData createForecastData);


        Task<ForecastScreen> GetForecastScreenBySkillGroupId(CreateForecastData createForecastData);

        void CreateForecastData(ForecastScreen forecastDataRequest);


        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        void UpdateForecastData(ForecastScreen forecastDataRequest);
        
    }
}
