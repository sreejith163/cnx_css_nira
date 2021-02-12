﻿using Css.Api.Core.Models.DTO.Response;
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
        Task<CSSResponse> GetForecastScreenBySkillGroupId(CreateForecastData createForecastData);

        Task<CSSResponse> GetForecastData(CreateForecastData createForecastData);
        
        /// <summary>
        /// Create Forecast screen data.
        /// </summary>
        /// <param name="forecastDataDetails">The agent schedule identifier details.</param>

        /// <returns></returns>
        Task<CSSResponse> CreateForecastData(CreateForecastData forecastDataDetails);

        /// <summary>
        /// Updates the Forecast Data
        /// </summary>

        /// <param name="createForecastData">the skill group id </param>
        /// <returns></returns>
        Task<CSSResponse> UpdateForecastData(CreateForecastData createForecastData, UpdateForecastData updateForecastData);

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        //Task<CSSResponse> UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleDetails);

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleManagerChartDetails">The agent schedule manager chart details.</param>
        /// <returns></returns>
        //Task<CSSResponse> UpdateAgentScheduleMangerChart(UpdateAgentScheduleManagerChart agentScheduleManagerChartDetails);

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        //Task<CSSResponse> ImportAgentScheduleChart(ImportAgentSchedule agentScheduleDetails);

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        //Task<CSSResponse> CopyAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule agentScheduleDetails);
    }
}