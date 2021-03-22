using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ForecastScreen;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IForecastScreenService
    {
        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="forecastScreenQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetForecastScreen(ForeCastScreenQueryParameter forecastScreenQueryparameter);

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetForecastScreenBySkillGroupId(ForecastIdDetails forecastIdDetails);

        Task<CSSResponse> GetForecastData(ForecastIdDetails forecastIdDetails);

        /// <summary>
        /// Create Forecast screen data.
        /// </summary>
        /// <param name="forecastDataDetails">The agent schedule identifier details.</param>

        /// <returns></returns>
        Task<CSSResponse> CreateForecastData(CreateForecastData forecastDataDetails);


        /// <summary>
        /// Imports the Forecast Data
        /// </summary>

        /// <param name="importForecastDetails">forecast import details</param>
        /// <returns></returns>
        Task<CSSResponse> ImportForecastData(ImportForecastDetails importForecastDetails);

        /// <summary>
        /// Updates the Forecast Data
        /// </summary>

        /// <param name="createForecastData">the skill group id </param>
        /// <returns></returns>
        Task<CSSResponse> UpdateForecastData(UpdateForecastData updateForecastData, int skillGroupId, string forecastDate);

        /// <summary>Gets the agent my schedule.</summary>
        /// <param name="skillGroupId">The employee identifier details.</param>
        /// <param name="date">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetAgentScheduledOpen(int skillGroupId, DateTimeOffset date);
    }
    }
