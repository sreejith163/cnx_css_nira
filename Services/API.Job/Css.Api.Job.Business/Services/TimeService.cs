using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Job.Business.Interfaces;
using Css.Api.Job.Models.DTO.Common;
using Css.Api.Job.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Job.Business.Services
{
    /// <summary>
    /// A helper service for time related operations
    /// </summary>
    public class TimeService : ITimeService
    {
        #region Private Properties

        /// <summary>
        /// The timezone repository
        /// </summary>
        private readonly ITimezoneRepository _timezoneRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="timezoneRepository"></param>
        public TimeService(ITimezoneRepository timezoneRepository)
        {
            _timezoneRepository = timezoneRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// A helper method to retreive all time information for the input span filters
        /// </summary>
        /// <param name="spanFilters">List of span filters</param>
        /// <returns>List of instances of SpanDetails</returns>
        public async Task<List<SpanDetails>> GetTimeDetails(List<SpanFilter> spanFilters)
        {
            List<SpanDetails> selectedTimes = new List<SpanDetails>();
            var timezones = await _timezoneRepository.GetTimezones();
            spanFilters.ForEach(spanFilter =>
            {
                var timeDetails = (from t in timezones
                                   where DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value).TimeOfDay >= spanFilter.Start
                                   && DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value).TimeOfDay < spanFilter.End
                                   && (!spanFilter.Include.Any() || spanFilter.Include.Contains(t.TimezoneId))
                                   && (!spanFilter.Exclude.Any() || !spanFilter.Exclude.Contains(t.TimezoneId))
                                   select new SpanDetails
                                   {
                                       Days = spanFilter.DaysOfData,
                                       CurrentDate = DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value).Date,
                                       CurrentDateTime = DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value),
                                       TimezoneId = t.TimezoneId
                                   }).ToList();

                if(timeDetails.Any())
                {
                    selectedTimes.AddRange(timeDetails);
                }
            });
            
            return selectedTimes;
        }

        /// <summary>
        /// A helper method to retreive all time information for the input intra day filters
        /// </summary>
        /// <param name="intraDayFilters">List of intra day filters</param>
        /// <returns>List of instances of IntraDayDetails</returns>
        public async Task<List<IntraDayDetails>> GetTimeDetails(List<IntraDayFilter> intraDayFilters)
        {
            List<IntraDayDetails> selectedTimes = new List<IntraDayDetails>();
            var timezones = await _timezoneRepository.GetTimezones();
            intraDayFilters.ForEach(intraDayFilter =>
            {
                var timeDetails = (from t in timezones
                                   where (!intraDayFilter.Include.Any() || intraDayFilter.Include.Contains(t.TimezoneId))
                                   && (!intraDayFilter.Exclude.Any() || !intraDayFilter.Exclude.Contains(t.TimezoneId))
                                   select new IntraDayDetails
                                   {
                                       TimezoneId = t.TimezoneId,
                                       CurrentDate = DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value).Date,
                                       CurrentDateTime = DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value),
                                       StartDate = DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value).Date.AddDays(intraDayFilter.RelativeStartDay),
                                       EndDate = DateTime.UtcNow.Add(TimezoneHelper.GetOffset(t.Abbreviation).Value).Date.AddDays(intraDayFilter.RelativeEndDay),
                                       RecentlyUpdatedInDays = intraDayFilter.PickRecentlyUpdatedInDays
                                   }).ToList();

                if (timeDetails.Any())
                {
                    selectedTimes.AddRange(timeDetails);
                }
            });
            return selectedTimes;
        }
        #endregion
    }
}
