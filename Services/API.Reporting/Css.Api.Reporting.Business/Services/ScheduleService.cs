using AutoMapper;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Common;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.Common;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The schedule clock helper service
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        #region Private Properties

        /// <summary>
        /// The automapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The agent repository
        /// </summary>
        private readonly IAgentRepository _agentRepository;

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentScheduleRepository _agentScheduleRepository;

        /// <summary>
        /// The agent schedule manager repository
        /// </summary>
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The agent scheduling group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>
        /// The scheduling code repository
        /// </summary>
        private readonly ISchedulingCodeRepository _schedulingCodeRepository;

        /// <summary>
        /// The timezone repository
        /// </summary>
        private readonly ITimezoneRepository _timezoneRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="agentRepository"></param>
        /// <param name="agentScheduleRepository"></param>
        /// <param name="agentScheduleManagerRepository"></param>
        /// <param name="agentSchedulingGroupRepository"></param>
        /// <param name="schedulingCodeRepository"></param>
        /// <param name="timezoneRepository"></param>
        public ScheduleService(IMapper mapper, IAgentRepository agentRepository, IAgentScheduleRepository agentScheduleRepository,
            IAgentScheduleManagerRepository agentScheduleManagerRepository, IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            ISchedulingCodeRepository schedulingCodeRepository, ITimezoneRepository timezoneRepository)
        {
            _mapper = mapper;
            _agentRepository = agentRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _schedulingCodeRepository = schedulingCodeRepository;
            _timezoneRepository = timezoneRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to generate instances of calendar charts using the filter params
        /// </summary>
        /// <param name="filter">The filters to be applied</param>
        /// <returns>The list of instances of CalendarChart</returns>
        public async Task<List<CalendarChart>> GetCalendarCharts(ScheduleFilter filter)
        {
            List<AgentSchedulingGroup> schedulingGroups = new List<AgentSchedulingGroup>();
            if (!filter.AgentSchedulingGroupIds.Any())
            {
                schedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroups(filter.TimezoneIds, filter.EstartProvision);
                filter.AgentSchedulingGroupIds = schedulingGroups.Select(x => x.AgentSchedulingGroupId).ToList();
            }
            else
            {
                schedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupsByIds(filter.AgentSchedulingGroupIds, filter.EstartProvision);
                filter.TimezoneIds = schedulingGroups.Select(x => x.TimezoneId).Distinct().ToList();
            }
            List<CalendarChart> charts = new List<CalendarChart>();
            if (!schedulingGroups.Any())
            {
                return charts;
            }

            List<AgentScheduleManager> schedules = await _agentScheduleManagerRepository.GetManagerSchedules(filter);

            if (schedules.Any())
            {
                var timezones = await _timezoneRepository.GetTimezones(filter.TimezoneIds);
                List<TimezoneDetails> timezoneDetails = _mapper.Map<List<TimezoneDetails>>(timezones);
                List<SchedulingGroupDetails> schedulingGroupDetailsList = GetSchedulingGroupDetails(schedulingGroups, timezoneDetails);

                var codes = await _schedulingCodeRepository.GetSchedulingCodes();
                charts.AddRange(GenerateCalendarCharts(schedules, schedulingGroupDetailsList, codes));
            }

            return charts.OrderBy(x => x.EmployeeId).ThenBy(x => x.ScheduledDate).ToList();
        }

        /// <summary>
        /// The method to generate instances of calendar charts using the agent ids and other filter params
        /// </summary>
        /// <param name="filter">The filters to be applied</param>
        /// <returns>The list of instances of CalendarChart</returns>
        public async Task<List<CalendarChart>> GetCalendarChartsUsingIds(ScheduleFilter filter)
        {
            List<CalendarChart> charts = new List<CalendarChart>();

            if (!filter.AgentIds.Any())
            {
                return charts;
            }

            List<AgentScheduleManager> schedules = await _agentScheduleManagerRepository.GetManagerSchedules(filter);
            schedules = schedules.OrderBy(x => x.EmployeeId).ThenBy(x => x.Date).ToList();
            if (schedules.Any())
            {
                var agents = await _agentRepository.GetAgents(filter.AgentIds);
                var asgs = agents.Select(x => x.AgentSchedulingGroupId)
                                .Union(schedules.Select(x => x.AgentSchedulingGroupId))
                                .Distinct()
                                .ToList();
                var schedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupsByIds(asgs);
                filter.TimezoneIds = schedulingGroups.Select(x => x.TimezoneId).Distinct().ToList();
                var timezones = await _timezoneRepository.GetTimezones(filter.TimezoneIds);
                List<TimezoneDetails> timezoneDetails = _mapper.Map<List<TimezoneDetails>>(timezones);
                List<SchedulingGroupDetails> schedulingGroupDetailsList = GetSchedulingGroupDetails(schedulingGroups, timezoneDetails);

                var codes = await _schedulingCodeRepository.GetSchedulingCodes();
                charts.AddRange(GenerateCalendarCharts(schedules, schedulingGroupDetailsList, codes));
            }

            return charts.OrderBy(x => x.EmployeeId).ThenBy(x => x.ScheduledDate).ToList();
        }

        /// <summary>
        /// The method to parse the clocks text to instances of calender charts
        /// </summary>
        /// <param name="chartsText"></param>
        /// <param name="invalidData"></param>
        /// <param name="estartProvisioning"></param>
        /// <returns>A list of instances of ScheduleClock. All unparsed/invalid data is added to the 'invalidData' parameter</returns>
        public List<CalendarChart> ParseCalenderCharts(string chartsText, out List<string> invalidData, bool? estartProvisioning = null)
        {
            List<CalendarChartData> chartDataList = ParseChartText(chartsText);

            var parsedChartList = chartDataList.Where(x => x.PatternParseStatus).ToList();
            var schedulingCodes = _schedulingCodeRepository.GetSchedulingCodesByNames(parsedChartList.Select(x => x.ActText).ToList()).Result;
            var empIds = parsedChartList.Select(x =>
                    {
                        string value = x.EmpNo;
                        bool success = true;
                        return new { value, success };
                    })
                    .Where(pair => pair.success)
                    .Select(pair => pair.value)
                    .Distinct()
                    .ToList();

            var agents = _agentRepository.GetAgents(empIds).Result;
            Dictionary<string, List<DateTime>> invalidEmpDates = empIds.Distinct()
                                        .Select(x => {
                                            return new { key = x, value = new List<DateTime>() };
                                        })
                                        .ToDictionary(x => x.key, y => y.value);

            ParseCalenderChartData(parsedChartList, schedulingCodes, agents, invalidEmpDates);

            List<CalendarChartData> invalidClockData = chartDataList.Where(x => x.Chart == null || x.PatternParseStatus == false).ToList();

            invalidEmpDates.Keys.ToList().ForEach(empNo =>
            {
                invalidClockData.AddRange(chartDataList.Where(x => x.Chart != null && invalidEmpDates[empNo].Contains(x.Chart.ScheduledDate)).ToList());
            });
            invalidClockData.AddRange(CheckOverlappingClocks(chartDataList.Where(x => x.Chart != null).ToList()));

            chartDataList.RemoveAll(x => invalidClockData.Select(y => y.LineNo).Contains(x.LineNo));

            if (estartProvisioning.HasValue)
            {
                var agentSchedulingGroups = _agentSchedulingGroupRepository.GetAgentSchedulingGroupsByIds(agents.Select(x => x.AgentSchedulingGroupId).ToList()).Result;
                var disabledSchedulingGroups = agentSchedulingGroups.Where(x => !x.EstartProvision).Select(x => x.AgentSchedulingGroupId).ToList();
                var invalid = chartDataList.Where(x => disabledSchedulingGroups.Contains(x.Chart.AgentSchedulingGroupId)).ToList();
                if(invalid.Any())
                {
                    invalidClockData.AddRange(invalid);
                    chartDataList.RemoveAll(x => invalidClockData.Select(y => y.LineNo).Contains(x.LineNo));
                }
            }

            invalidData = invalidClockData.Distinct().OrderBy(x => x.LineNo).Select(x => x.Data).ToList();
            return chartDataList.Select(x => x.Chart).OrderBy(x => x.EmployeeId).ThenBy(x => x.ScheduledDate).ToList();
        }

        /// <summary>
        /// The method to generate a text string for the charts
        /// </summary>
        /// <param name="charts">The list of instances of CalendarChart</param>
        /// <returns>The text string</returns>
        public string GenerateExportText(List<CalendarChart> charts)
        {
            string clockText = "";
            charts.ForEach(x =>
            {
                clockText += string.Join('|', x.StartDateTime.ToString("yyyyMMdd"), x.ScheduledDate.ToString("yyyyMMdd"), x.EmployeeId, x.ActivityName, x.StartDateTime.ToString("HH:mm"), x.EndDateTime.ToString("HH:mm")) + "\n";
            });
            return clockText;
        }

        /// <summary>
        /// A helper method to parse the schedule manager details from the input schedule clocks
        /// </summary>
        /// <param name="charts"></param>
        /// <param name="activityInformation"></param>
        /// <returns>The list of instances of AgentScheduleManager</returns>
        public async Task<List<ScheduleManagerDetails>> GenerateAgentScheduleManagerCharts(List<CalendarChart> charts, ActivityInformation activityInformation)
        {
            var employees = charts.Select(x => x.EmployeeId).Distinct().ToList();
            var dates = charts.Select(x => x.ScheduledDate).Distinct().ToList();
            var currentTimestamp = DateTime.UtcNow;
            List<ScheduleManagerDetails> agentScheduleManagerList = new List<ScheduleManagerDetails>();
            List<AgentScheduleManager> existingSchedules = await _agentScheduleManagerRepository.GetManagerSchedules(employees, dates);

            employees.ForEach(empId =>
            {
                var empClocks = charts.Where(x => x.EmployeeId == empId).OrderBy(x => x.ScheduledDate).ToList();
                var scheduleDates = empClocks.Select(x => x.ScheduledDate).Distinct().ToList();
                int empSchedulingGroup = empClocks.Select(x => x.AgentSchedulingGroupId).FirstOrDefault();

                scheduleDates.ForEach(date =>
                {
                    var empDateClocks = empClocks.Where(x => x.ScheduledDate.Equals(date)).OrderBy(x => x.StartDateTime).ToList();
                    var existingEmpSchedule = existingSchedules.FirstOrDefault(x => x.EmployeeId == empId && x.Date == date);
                    DateTime? modifiedDate = null;
                    if (existingEmpSchedule != null)
                    {
                        modifiedDate = currentTimestamp;
                    }

                    ScheduleManagerDetails agentScheduleManager = new ScheduleManagerDetails()
                    {
                        EmployeeId = empId,
                        ScheduledDate = date,
                        AgentSchedulingGroupId = empSchedulingGroup,
                        ExistingSchedule = existingEmpSchedule != null,
                        Schedules = new List<AgentScheduleManagerChart>(),
                        CreatedDate = currentTimestamp,
                        ModifiedDate = modifiedDate
                    };

                    empDateClocks.ForEach(clock => {
                        agentScheduleManager.Schedules.Add(new AgentScheduleManagerChart
                        {
                            StartDateTime = clock.StartDateTime,
                            EndDateTime = clock.EndDateTime,
                            SchedulingCodeId = clock.ActivityId
                        });
                    });

                    agentScheduleManager.Schedules = agentScheduleManager.Schedules.OrderBy(x => x.StartDateTime).ToList();

                    if (agentScheduleManager.Schedules.Any())
                    {
                        agentScheduleManagerList.Add(agentScheduleManager);
                    }
                });

            });

            return agentScheduleManagerList.Select(x => {
                x.CreatedBy = activityInformation.Origin.ToString();
                x.ModifiedBy = x.ExistingSchedule ? activityInformation.Origin.ToString() : null;
                x.Origin = activityInformation.Origin;
                x.Status = x.ExistingSchedule ? activityInformation.Status : ActivityStatus.Created;
                x.Type = activityInformation.Type;
                return x;
            }).ToList();
        }

        /// <summary>
        /// The method to generate activity agent schedules from the calendar charts
        /// </summary>
        /// <param name="calendarCharts"></param>
        /// <returns>The list of instances of AgentActivitySchedule</returns>
        public List<AgentActivitySchedule> GenerateActivityAgentSchedules(List<CalendarChart> calendarCharts)
        {
            List<AgentActivitySchedule> agentActivitySchedules = new List<AgentActivitySchedule>();
            var employees = calendarCharts.Select(x => x.EmployeeId).Distinct().OrderBy(x => x).ToList();

            employees.ForEach(emp =>
            {
                AgentActivitySchedule agentActivitySchedule = new AgentActivitySchedule()
                {
                    AgentId = emp,
                    BaseSchedule = new List<ActivitySchedule>()
                };
                
                var dates = calendarCharts.Where(x => x.EmployeeId == emp).Select(x => x.ScheduledDate).Distinct().ToList();
                dates.ForEach(date => 
                {
                    var dateCharts = calendarCharts.Where(x => x.EmployeeId == emp && x.ScheduledDate == date).OrderBy(x => x.StartDateTime).ToList();
                    if(dateCharts.Any())
                    {
                        var timezone = dateCharts.First().Timezone;
                        var schedule = new ActivitySchedule()
                        {
                            Timezone = timezone,
                            ScheduleDetail = new List<ActivityScheduleDetail>(),
                            ScheduleDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        };
                        dateCharts.ForEach(chart =>
                        {
                            schedule.ScheduleDetail.Add(new ActivityScheduleDetail()
                            {
                                ActivityDesc = chart.ActivityName,
                                StartDateTime = chart.StartDateTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                                EndDateTime = chart.EndDateTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                            });
                        });
                        agentActivitySchedule.BaseSchedule.Add(schedule);
                    }
                });
                
                if (agentActivitySchedule.BaseSchedule.Any())
                {
                    agentActivitySchedules.Add(agentActivitySchedule);
                }
            });

            return agentActivitySchedules;
        }

        /// <summary>
        /// A helper method to parse activity schedules
        /// </summary>
        /// <param name="activityScheduleUpdate"></param>
        /// <returns>An instance of ScheduleData</returns>
        public async Task<ScheduleData> ParseActivitySchedule(ActivityScheduleUpdate activityScheduleUpdate)
        {
            ScheduleData scheduleData = new ScheduleData()
            {
                Messages = new List<string>()
            };
            
            DateTime scheduledDate;
            var scheduleDateParse = DateTime.TryParseExact(activityScheduleUpdate.ScheduleDate, "yyyy-MM-dd" ,CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out scheduledDate);
            if(!scheduleDateParse)
            {
                scheduleData.Messages.Add(Messages.InvalidScheduleDate);
            }

            var agentInfo = await _agentRepository.GetAgents(new List<string>() { activityScheduleUpdate.AgentId });
            if (!agentInfo.Any())
            {
                scheduleData.Messages.Add(Messages.AgentNotFound);
            }

            var timeOffset = TimezoneHelper.GetOffset(activityScheduleUpdate.Timezone);
            if(!timeOffset.HasValue)
            {
                scheduleData.Messages.Add(Messages.InvalidTimezone);
            }

            if(scheduleData.Messages.Any())
            {
                return scheduleData;
            }

            scheduledDate = new DateTime(scheduledDate.Year, scheduledDate.Month, scheduledDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var agent = agentInfo.First();
            
            var existingSchedule = await _agentScheduleManagerRepository.GetManagerSchedules(new List<string>() { agent.Ssn }, new List<DateTime> { scheduledDate });
            TimeSpan offset;
            var dateTimeNow = DateTime.UtcNow;
            DateTime? modifiedDate = dateTimeNow;

            if (existingSchedule.Any())
            {
                var agentSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupsById(existingSchedule.First().AgentSchedulingGroupId);
                var agentTimezone = await _timezoneRepository.GetTimezone(agentSchedulingGroup.TimezoneId);
                offset = TimezoneHelper.GetOffset(agentTimezone.Abbreviation).Value;
            }
            else
            {
                var agentSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupsById(agent.AgentSchedulingGroupId);
                var agentTimezone = await _timezoneRepository.GetTimezone(agentSchedulingGroup.TimezoneId);
                offset = TimezoneHelper.GetOffset(agentTimezone.Abbreviation).Value;
                modifiedDate = null;
            }
            

            if(scheduledDate.Date <= DateTime.UtcNow.Add(offset).Date)
            {
                scheduleData.Messages.Add(Messages.ScheduleUpdateNotAllowed);
            }

            if (scheduleData.Messages.Any())
            {
                return scheduleData;
            }

            var offsetDiff = offset.Subtract(timeOffset.Value);

            ScheduleManagerData scheduleManagerData =  await ParseActivityScheduleUpdateDetail(agent.Ssn, scheduledDate, activityScheduleUpdate.ScheduleActivities);
            scheduleData.Messages.AddRange(scheduleManagerData.Messages);

            if(scheduleData.Messages.Any())
            {
                return scheduleData;
            }

            scheduleManagerData.ManagerCharts.ForEach(chart =>
            {
                chart.StartDateTime = chart.StartDateTime.Add(offsetDiff);
                chart.EndDateTime = chart.EndDateTime.Add(offsetDiff);
            });

            
            

            scheduleData.Schedule = new AgentScheduleManager()
            {
                EmployeeId = agent.Ssn,
                AgentSchedulingGroupId = agent.AgentSchedulingGroupId,
                CreatedDate = dateTimeNow,
                CreatedBy = ActivityOrigin.CNX1.ToString(),
                ModifiedDate = modifiedDate,
                ModifiedBy = existingSchedule.Any() ? ActivityOrigin.CNX1.ToString() : null,
                Date = scheduledDate,
                Charts = scheduleManagerData.ManagerCharts
            };

            return scheduleData;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// A method to map the agent scheduling groups with their respective timezone details
        /// </summary>
        /// <param name="agentSchedulingGroups"></param>
        /// <param name="timezoneDetails"></param>
        /// <returns></returns>
        private List<SchedulingGroupDetails> GetSchedulingGroupDetails(List<AgentSchedulingGroup> agentSchedulingGroups, List<TimezoneDetails> timezoneDetails)
        {
            return (from s in agentSchedulingGroups
                    join t in timezoneDetails on s.TimezoneId equals t.TimezoneId
                    select new SchedulingGroupDetails
                    {
                        AgentSchedulingGroupId = s.AgentSchedulingGroupId,
                        TimezoneId = t.TimezoneId,
                        TimezoneValue = t.TimezoneValue,
                        TimezoneOffsetValue = t.TimezoneOffsetValue
                    }).ToList();
        }

        /// <summary>
        /// A method to get the clocks for the agent manager tab charts
        /// </summary>
        /// <param name="agentScheduleManagers">The agent schedule manager list</param>
        /// <param name="schedulingGroupDetailsList">The scheduling group details list</param>
        /// <param name="codes">The scheduling codes in the system</param>
        /// <returns>List of instances of ScheduleClock matching the input criterias</returns>
        private List<CalendarChart> GenerateCalendarCharts(List<AgentScheduleManager> agentScheduleManagers, List<SchedulingGroupDetails> schedulingGroupDetailsList, List<SchedulingCode> codes)
        {
            List<CalendarChart> managerCharts = new List<CalendarChart>();
            agentScheduleManagers.ForEach(agentScheduleManager =>
            {
                var schedulingGroupDetail = schedulingGroupDetailsList.First(x => x.AgentSchedulingGroupId == agentScheduleManager.AgentSchedulingGroupId);

                managerCharts.AddRange((from c in agentScheduleManager.Charts
                                          join sc in codes on c.SchedulingCodeId equals sc.SchedulingCodeId
                                          select new CalendarChart
                                          {
                                              EmployeeId = agentScheduleManager.EmployeeId,
                                              ActivityId = c.SchedulingCodeId,
                                              ActivityName = sc.Name,
                                              ScheduledDate = agentScheduleManager.Date,
                                              AgentSchedulingGroupId = agentScheduleManager.AgentSchedulingGroupId,
                                              Timezone = schedulingGroupDetail.TimezoneValue,
                                              TimezoneOffset = schedulingGroupDetail.TimezoneOffsetValue,
                                              StartDateTime = c.StartDateTime,
                                              EndDateTime = c.EndDateTime
                                          }).ToList());
            });
            
            List<CalendarChart> calendarCharts = new List<CalendarChart>();
            managerCharts.ForEach(chart =>
            {
                var existingRec = calendarCharts.FirstOrDefault(x => x.EmployeeId == chart.EmployeeId
                                                    && x.ActivityId == chart.ActivityId
                                                    && x.ScheduledDate == chart.ScheduledDate
                                                    && x.EndDateTime == chart.StartDateTime);
                if (existingRec != null)
                {
                    existingRec.EndDateTime = chart.EndDateTime;
                }
                else
                {
                    calendarCharts.Add(chart);
                }
            });

            return calendarCharts.OrderBy(x => x.EmployeeId).ThenBy(x => x.ScheduledDate).ThenBy(x => x.StartDateTime).ToList();
        }

        /// <summary>
        /// A helper method to convert the time string to 24 hour clock time
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns>The 24 hour time string in the format 'hh:mm'</returns>
        private string ConvertTo24HrClock(string timeString)
        {
            string time24hr;
            var pattern = new Regex(@"(12:\d{2}){1}\s*((am|pm)|(a.m.|p.m.))");
            var matches = pattern.Match(timeString);
            if(matches.Success)
            {
                var splitTime = matches.Groups[1].Value.Split(":");
                if(timeString.Contains("pm"))
                {
                    time24hr = string.Join(":", splitTime[0], splitTime[1]);
                }
                else
                {
                    time24hr = string.Join(":", "00", splitTime[1]);
                }
                
            }
            else if (timeString.Contains("pm"))
            {
                var splitTime = timeString.Split(":");
                time24hr = string.Join(":", (int.Parse(splitTime[0]) + 12).ToString(), splitTime[1].Replace("pm", ""));
            }
            else
            {
                time24hr = timeString.Replace("am", "");
            }
            return time24hr.Trim();
        }

        /// <summary>
        /// A helper method to reconcile clocks for DateTime cross-overs
        /// </summary>
        /// <param name="clocks"></param>
        /// <param name="empIds"></param>
        /// <returns>A list of instances of ScheduleClock</returns>
        private List<ScheduleClock> ReconcileScheduleClocks(List<ScheduleClock> clocks, List<string> empIds)
        {
            List<ScheduleClock> reconciledClocks = new List<ScheduleClock>();

            empIds.ForEach(empNo =>
            {
                var empClocks = clocks.Where(x => x.EmployeeId == empNo).ToList();
                var schDates = empClocks.Select(x => x.ScheduledDate).Distinct().ToList();
                schDates.ForEach(date =>
                {
                    var empSchClocks = empClocks.Where(x => x.ScheduledDate == date).ToList();
                    var fiveMinClocks = Generate5MinClocks(empSchClocks);
                    reconciledClocks.AddRange(RegenerateClocks(fiveMinClocks));
                });
            });

            return reconciledClocks;
        }

        /// <summary>
        /// A helper method to reconcile the manager chart clocks based on new and existing clocks
        /// </summary>
        /// <param name="newClocks"></param>
        /// <param name="existingClocks"></param>
        /// <returns>A list of instances of ScheduleClock</returns>
        private List<ScheduleClock> ReconcileManagerClocks(List<ScheduleClock> newClocks, List<ScheduleClock> existingClocks)
        {
            var curr5minClocks = Generate5MinClocks(newClocks);
            var existing5minClocks = Generate5MinClocks(existingClocks);

            var common5minClocks = (from c in curr5minClocks
                                    join e in existing5minClocks on c.FromTime equals e.FromTime
                                    select new ScheduleClock
                                    {
                                        ActivityId = c.ActivityId,
                                        Date = c.Date,
                                        ScheduledDate = c.ScheduledDate,
                                        EmployeeId = c.EmployeeId,
                                        FromTime = c.FromTime,
                                        ToTime = c.ToTime
                                    }).ToList();
            
            curr5minClocks = curr5minClocks.Where(x => !common5minClocks.Any(y => y.FromTime == x.FromTime)).ToList();
            existing5minClocks = existing5minClocks.Where(x => !common5minClocks.Any(y => y.FromTime == x.FromTime)).ToList();
            var final5minClocks = curr5minClocks.Union(existing5minClocks).Union(common5minClocks).OrderBy(x => x.FromTime).ToList();
            
            return RegenerateClocks(final5minClocks);
        }

        /// <summary>
        /// A helper method to generate clocks with 5 min intervals
        /// </summary>
        /// <param name="clocks"></param>
        /// <returns>A list of instances of ScheduleClock</returns>
        private List<ScheduleClock> Generate5MinClocks(List<ScheduleClock> clocks)
        {
            List<ScheduleClock> fiveMinClocks = new List<ScheduleClock>();
            clocks.ForEach(x =>
            {
                var startTime = TimeSpan.Parse(x.FromTime);
                var endTime = TimeSpan.Parse(x.ToTime);
                if (startTime <= endTime)
                {
                    var fiveMinuteIntervals = (endTime.Subtract(startTime).TotalMinutes) / 5;
                    for (int i = 0; i < fiveMinuteIntervals; i++)
                    {
                        fiveMinClocks.Add(new ScheduleClock
                        {
                            ActivityId = x.ActivityId,
                            ActivityName = x.ActivityName,
                            Date = x.Date,
                            EmployeeId = x.EmployeeId,
                            ScheduledDate = x.ScheduledDate,
                            Timezone = x.Timezone,
                            FromTime = string.Format("{0:hh\\:mm}", startTime.Add(TimeSpan.FromMinutes(i * 5))),
                            ToTime = string.Format("{0:hh\\:mm}", startTime.Add(TimeSpan.FromMinutes((i + 1) * 5)))
                        });
                    }
                }
                else
                {
                    var eodTime = new TimeSpan(23, 59, 0);
                    var fiveMinuteIntervals = (eodTime.Subtract(startTime).TotalMinutes + 1) / 5;
                    for (int i = 0; i < fiveMinuteIntervals; i++)
                    {
                        fiveMinClocks.Add(new ScheduleClock
                        {
                            ActivityId = x.ActivityId,
                            ActivityName = x.ActivityName,
                            Date = x.ScheduledDate,
                            EmployeeId = x.EmployeeId,
                            ScheduledDate = x.ScheduledDate,
                            Timezone = x.Timezone,
                            FromTime = string.Format("{0:hh\\:mm}", startTime.Add(TimeSpan.FromMinutes(i * 5))),
                            ToTime = string.Format("{0:hh\\:mm}", startTime.Add(TimeSpan.FromMinutes((i + 1) * 5)))
                        });
                    }
                    var minutesToAdd = fiveMinuteIntervals * 5;
                    fiveMinuteIntervals = endTime.TotalMinutes / 5;
                    for (int i = 0; i < fiveMinuteIntervals; i++)
                    {
                        fiveMinClocks.Add(new ScheduleClock
                        {
                            ActivityId = x.ActivityId,
                            ActivityName = x.ActivityName,
                            Date = x.Date,
                            EmployeeId = x.EmployeeId,
                            ScheduledDate = x.ScheduledDate,
                            Timezone = x.Timezone,
                            FromTime = string.Format("{0:hh\\:mm}", startTime.Add(TimeSpan.FromMinutes(minutesToAdd + (i * 5)))),
                            ToTime = string.Format("{0:hh\\:mm}", startTime.Add(TimeSpan.FromMinutes(minutesToAdd + ((i + 1) * 5))))
                        });
                    }
                }
            });
            return fiveMinClocks;
        }

        /// <summary>
        /// A helper method to regenerate clocks using 5 min clocks
        /// </summary>
        /// <param name="fiveMinClocks"></param>
        /// <returns>A list of instances of ScheduleClock</returns>
        private List<ScheduleClock> RegenerateClocks(List<ScheduleClock> fiveMinClocks)
        {
            List<ScheduleClock> clocks = new List<ScheduleClock>();
            if(fiveMinClocks.Any())
            {
                var currClock = fiveMinClocks[0];
                string startTime = fiveMinClocks[0].FromTime;
                
                for (int i = 1; i < fiveMinClocks.Count; i++)
                {
                    var clock = fiveMinClocks[i];
                    if(clock.ToTime == "00:00")
                    {
                        clocks.Add(new ScheduleClock
                        {
                            ActivityId = currClock.ActivityId,
                            ActivityName = currClock.ActivityName,
                            Date = currClock.Date,
                            EmployeeId = currClock.EmployeeId,
                            ScheduledDate = currClock.ScheduledDate,
                            Timezone = currClock.Timezone,
                            FromTime = startTime,
                            ToTime = clock.ToTime
                        });
                        currClock = clock;
                        startTime = currClock.ToTime;
                    }
                    else
                    {
                        var timeDiff = TimeSpan.Parse(clock.ToTime).Subtract(TimeSpan.Parse(currClock.ToTime)).TotalMinutes;
                        if (clock.ActivityId == currClock.ActivityId && timeDiff == 5)
                        {
                            currClock = clock;
                            if (i == fiveMinClocks.Count - 1)
                            {
                                clocks.Add(new ScheduleClock
                                {
                                    ActivityId = currClock.ActivityId,
                                    ActivityName = currClock.ActivityName,
                                    Date = currClock.Date,
                                    EmployeeId = currClock.EmployeeId,
                                    ScheduledDate = currClock.ScheduledDate,
                                    Timezone = currClock.Timezone,
                                    FromTime = startTime,
                                    ToTime = currClock.ToTime
                                });
                            }
                        }
                        else
                        {
                            clocks.Add(new ScheduleClock
                            {
                                ActivityId = currClock.ActivityId,
                                ActivityName = currClock.ActivityName,
                                Date = currClock.Date,
                                EmployeeId = currClock.EmployeeId,
                                ScheduledDate = currClock.ScheduledDate,
                                Timezone = currClock.Timezone,
                                FromTime = startTime,
                                ToTime = currClock.ToTime
                            });
                            currClock = clock;
                            startTime = currClock.FromTime;
                        }
                    }
                        
                }
            }
            return clocks;
        }

        /// <summary>
        /// A helper method to parse the text based on a pattern of
        /// 'YYYYMMDD|YYYYMMDD|12345|Code|HH:MM|HH:MM'
        /// </summary>
        /// <param name="content"></param>
        /// <returns>A list of instances of CalendarChartData</returns>
        private List<CalendarChartData> ParseChartText(string content)
        {
            var contentLine = Regex.Split(content, "\r\n|\r|\n").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            List<CalendarChartData> chartDataList = new List<CalendarChartData>();
            int lineCount = 0;

            contentLine.ForEach(line =>
            {
                lineCount++;
                var pattern = new Regex(@"^(?'actDate'\d{4}((0\d)|(1[012]))(([012]\d)|3[01]))[|](?'schDate'\d{4}((0\d)|(1[012]))(([012]\d)|3[01]))[|](?'empNo'\d+)[|](?'actText'(\w*\s*)*)[|](?'startTime'([01]?[0-9]|2[0-3]):[0-5][0-9])[|](?'endTime'([01]?[0-9]|2[0-3]):[0-5][0-9])$");
                var match = pattern.Match(line.Trim());

                if (match.Success)
                {
                    var matchGroups = match.Groups.Values.ToList();
                    chartDataList.Add(new CalendarChartData
                    {
                        LineNo = lineCount,
                        Data = line,
                        EmpNo = matchGroups.Where(x => x.Name == "empNo").Select(x => x.Value).FirstOrDefault() ?? "",
                        ActDate = matchGroups.Where(x => x.Name == "actDate").Select(x => x.Value).FirstOrDefault() ?? "",
                        SchDate = matchGroups.Where(x => x.Name == "schDate").Select(x => x.Value).FirstOrDefault() ?? "",
                        ActText = matchGroups.Where(x => x.Name == "actText").Select(x => x.Value).FirstOrDefault() ?? "",
                        ActStartTime = matchGroups.Where(x => x.Name == "startTime").Select(x => x.Value).FirstOrDefault() ?? "",
                        ActEndTime = matchGroups.Where(x => x.Name == "endTime").Select(x => x.Value).FirstOrDefault() ?? "",
                        PatternParseStatus = true
                    });
                }
                else
                {
                    chartDataList.Add(new CalendarChartData
                    {
                        LineNo = lineCount,
                        Data = line,
                        PatternParseStatus = false
                    });
                }
            });

            return chartDataList;
        }

        /// <summary>
        /// A helper method to parse and set properties of the input ScheduleClockData list using other input parameters
        /// </summary>
        /// <param name="calendarChartDatas"></param>
        /// <param name="schedulingCodes"></param>
        /// <param name="agents"></param>
        private void ParseCalenderChartData(List<CalendarChartData> calendarChartDatas, List<SchedulingCode> schedulingCodes, List<Agent> agents, Dictionary<string, List<DateTime>> invalidEmpDates)
        {
            calendarChartDatas.ForEach(calendarChartData =>
            {
                DateTime schDate, startDateTime = DateTime.MinValue, endDateTime = DateTime.MinValue;
                TimeSpan startTime, endTime;
                int empNo = 0;
                string employeeNo = calendarChartData.EmpNo;
                var code = schedulingCodes.FirstOrDefault(x => x.Name.Equals(calendarChartData.ActText.Trim()));

                bool empStatus = (int.TryParse(calendarChartData.EmpNo, out empNo) || empNo == 0);
                bool empValueStatus = agents.Any(x => x.Ssn.Equals(empNo));
                bool actStatus = DateTime.TryParseExact(calendarChartData.ActDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out schDate);
                bool schStatus = DateTime.TryParseExact(calendarChartData.SchDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out schDate);
                bool actTextStatus = !string.IsNullOrWhiteSpace(calendarChartData.ActText);
                bool codeStatus = code != null;
                bool allDateStatus = false;
                bool startTimeStatus = TimeSpan.TryParseExact(string.Join(":",calendarChartData.ActStartTime,"00"), "g", CultureInfo.InvariantCulture, out startTime);
                bool endTimeStatus = TimeSpan.TryParseExact(string.Join(":",calendarChartData.ActEndTime,"00"), "g", CultureInfo.InvariantCulture, out endTime);
                
                if (empStatus && schStatus && startTimeStatus && endTimeStatus)
                {
                    bool startStatus = false;
                    bool endStatus = false;
                    string actStartDateTime = string.Join(" ", calendarChartData.ActDate, calendarChartData.ActStartTime);
                    string actEndDateTime = string.Join(" ", calendarChartData.ActDate, calendarChartData.ActEndTime);

                    startStatus = DateTime.TryParseExact(actStartDateTime, "yyyyMMdd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime);
                    endStatus = DateTime.TryParseExact(actEndDateTime, "yyyyMMdd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime);

                    if (startTime > endTime && endStatus)
                    {
                        endDateTime = endDateTime.AddDays(1);
                    }

                    if (startStatus && endStatus && startDateTime < endDateTime && ValidateScheduleDateClocks(schDate, startDateTime, endDateTime))
                    {
                        allDateStatus = true;
                    }
                    else
                    {
                        // invalidEmpDates[empNo].Add(schDate);
                    }
                }

                //if(!codeStatus && schStatus)
                //{
                //    invalidEmpDates[empNo].Add(schDate);
                //}

                bool parseStatus = empStatus && empValueStatus && schStatus && actTextStatus 
                        && codeStatus && allDateStatus;

                if (parseStatus)
                { 
                    calendarChartData.Chart = new CalendarChart()
                    {
                        EmployeeId = employeeNo,
                        AgentSchedulingGroupId = agents.First(x => x.Ssn == employeeNo).AgentSchedulingGroupId,
                        ScheduledDate = new DateTime(schDate.Year, schDate.Month, schDate.Day, 0, 0, 0, DateTimeKind.Utc),
                        ActivityId = code.SchedulingCodeId,
                        ActivityName = calendarChartData.ActText.Trim(),
                        StartDateTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, startDateTime.Hour, startDateTime.Minute, startDateTime.Second, DateTimeKind.Utc),
                        EndDateTime = new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, endDateTime.Hour, endDateTime.Minute, endDateTime.Second, DateTimeKind.Utc)
                    };
                }
            });
        }

        /// <summary>
        /// The helper method to parse schedule update detail for the scheduled date
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="scheduledDate"></param>
        /// <param name="activitySchedules"></param>
        /// <returns>An instance of ScheduleManagerData</returns>
        private async Task<ScheduleManagerData> ParseActivityScheduleUpdateDetail(string agentId, DateTime scheduledDate, List<ActivityScheduleUpdateDetail> activitySchedules)
        {
            ScheduleManagerData scheduleManagerData = new ScheduleManagerData()
            {
                ManagerCharts = new List<AgentScheduleManagerChart>(),
                Messages = new List<string>()
            };
            
            var codes = await _schedulingCodeRepository.GetSchedulingCodesByNames(activitySchedules.Select(x => x.ActivityDesc).Distinct().ToList());
            activitySchedules.ForEach(schedule =>
            {
                DateTime startDateTime, endDateTime;
                var code = codes.FirstOrDefault(x => x.Name.Equals(schedule.ActivityDesc.Trim()));
                
                bool startStatus = DateTime.TryParseExact(schedule.StartDateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime);
                if(!startStatus)
                {
                    scheduleManagerData.Messages.Add(string.Format(Messages.InvalidDateTimeFormat, schedule.StartDateTime));
                }

                bool endStatus = DateTime.TryParseExact(schedule.EndDateTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime);
                if(!endStatus)
                {
                    scheduleManagerData.Messages.Add(string.Format(Messages.InvalidDateTimeFormat, schedule.EndDateTime));
                }

                bool codeStatus = code != null;
                if (!codeStatus)
                {
                    scheduleManagerData.Messages.Add(string.Format(Messages.InvalidSchedulingCode, schedule.ActivityDesc));
                }

                bool allDateStatus = true;
                if (startStatus && endStatus)
                {
                    bool validateSchedule = ValidateScheduleDateClocks(scheduledDate, startDateTime, endDateTime);
                    if (startDateTime > endDateTime)
                    {
                        allDateStatus = false;
                        scheduleManagerData.Messages.Add(string.Format(Messages.InvalidEndDateTime, schedule.EndDateTime, schedule.StartDateTime));
                    }
                    else if(!validateSchedule)
                    {
                        allDateStatus = false;
                        scheduleManagerData.Messages.Add(Messages.InvalidDateTimeSchedule);
                    }    
                }

                bool parseStatus = startStatus && endStatus && codeStatus && allDateStatus;
                
                if(parseStatus)
                {
                    scheduleManagerData.ManagerCharts.Add(new AgentScheduleManagerChart
                    {
                        SchedulingCodeId = code.SchedulingCodeId,
                        EndDateTime = new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, endDateTime.Hour, endDateTime.Minute, endDateTime.Second, DateTimeKind.Utc),
                        StartDateTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, startDateTime.Hour, startDateTime.Minute, endDateTime.Second, DateTimeKind.Utc)
                    });
                }
            });

            if(scheduleManagerData.Messages.Any())
            {
                scheduleManagerData.ManagerCharts = new List<AgentScheduleManagerChart>();
                return scheduleManagerData;
            }

            bool overlaps = !CheckNoOverlaps(scheduleManagerData.ManagerCharts);
            if(overlaps)
            {
                scheduleManagerData.Messages.Add(string.Format(Messages.OverlappingSchedules, scheduledDate.ToString("yyyy-MM-dd")));
                scheduleManagerData.ManagerCharts = new List<AgentScheduleManagerChart>();
            }

            List<string> scheduleOverlapsMessages = await CheckOverlapsInExistingSchedules(agentId, scheduledDate, scheduleManagerData.ManagerCharts);
            if(scheduleOverlapsMessages.Any())
            {
                scheduleManagerData.Messages.AddRange(scheduleOverlapsMessages);
                scheduleManagerData.ManagerCharts = new List<AgentScheduleManagerChart>();
            }
            return scheduleManagerData;
        }

        /// <summary>
        /// A helper method to check and return any overlapping schedules within the schedule clock data for a day
        /// </summary>
        /// <param name="scheduleClockDatas"></param>
        /// <returns>A list of instances of ScheduleClockData which has atleast one overrlapping schedule </returns>
        private List<CalendarChartData> CheckOverlappingClocks(List<CalendarChartData> scheduleClockDatas)
        {
            List<CalendarChartData> overlappingClocks = new List<CalendarChartData>();
            var employees = scheduleClockDatas.Select(x => x.Chart.EmployeeId).Distinct().ToList();
            employees.ForEach(emp =>
            {
                var empClockData = scheduleClockDatas.Where(x => x.Chart.EmployeeId == emp).ToList();
                var empClockDays = empClockData.Select(x => x.Chart.ScheduledDate).Distinct().ToList();
                empClockDays.ForEach(async day =>
                {
                    var empDayClockData = empClockData.Where(x => x.Chart.ScheduledDate == day)
                                                .OrderBy(x => x.ActStartTime)
                                                .ThenBy(x => x.ActEndTime)
                                                .ToList();
                    
                    if (empDayClockData.Any())
                    {
                        var overlaps = !CheckNoOverlaps(empDayClockData.Select(x => x.Chart).ToList());
                        if(overlaps)
                        {
                            overlappingClocks.AddRange(empDayClockData);
                        }
                        else
                        {
                            var adjacentCalendars = empClockData.Where(x => x.Chart.ScheduledDate == day.AddDays(-1) || x.Chart.ScheduledDate == day.AddDays(1)).ToList();
                            if(adjacentCalendars.Any())
                            {
                                if(empDayClockData.Any(x => CheckIfOverlapSchedulesExists(x.Chart, adjacentCalendars.Select(x => x.Chart).ToList())))
                                {
                                    overlappingClocks.AddRange(empDayClockData);
                                }
                            }
                            else
                            {
                                var adjacentDaySchedules = await _agentScheduleManagerRepository.GetManagerSchedules(new List<string>() { emp }, new List<DateTime>() { day.AddDays(-1), day.AddDays(1) });
                                if (empDayClockData.Any(x => CheckIfOverlapSchedulesExists(x.Chart, adjacentDaySchedules)))
                                {
                                    overlappingClocks.AddRange(empDayClockData);
                                }
                            }
                            
                        }
                    }
                    
                });
                
            });

            return overlappingClocks;
        }

        /// <summary>
        /// The method to validate if dates fall in -1 to +1 day range
        /// </summary>
        /// <param name="scheduledDate"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        private bool ValidateScheduleDateClocks(DateTime scheduledDate, DateTime startDateTime, DateTime endDateTime)
        {
            var startDate = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, 0, 0, 0, DateTimeKind.Utc);
            var validStartDate = (startDate.AddDays(-1) == scheduledDate.Date || startDate.AddDays(1) == scheduledDate.Date || startDate == scheduledDate.Date);
            var validEndDate =  (endDate.AddDays(-1) == scheduledDate.Date || endDate.AddDays(1) == scheduledDate.Date || endDate == scheduledDate.Date);
            return validStartDate && validEndDate;
        }

        /// <summary>
        /// A method to check if there are any overlapping times for the input list
        /// </summary>
        /// <param name="calendarCharts"></param>
        /// <returns>True, if no overlaps</returns>
        private bool CheckNoOverlaps(List<CalendarChart> calendarCharts)
        {
            DateTime endPrior = DateTime.MinValue;
            foreach (CalendarChart chart in calendarCharts.OrderBy(x => x.StartDateTime))
            {
                if (chart.StartDateTime > chart.EndDateTime)
                    return false;
                if (chart.StartDateTime < endPrior)
                    return false;
                endPrior = chart.EndDateTime;
            }
            return true;
        }

        /// <summary>
        /// A method to check if there are any overlapping times for the input list
        /// </summary>
        /// <param name="managerCharts"></param>
        /// <returns>True, if no overlaps</returns>
        private bool CheckNoOverlaps(List<AgentScheduleManagerChart> managerCharts)
        {
            DateTime endPrior = DateTime.MinValue;
            foreach (AgentScheduleManagerChart chart in managerCharts.OrderBy(x => x.StartDateTime))
            {
                if (chart.StartDateTime > chart.EndDateTime)
                    return false;
                if (chart.StartDateTime < endPrior)
                    return false;
                endPrior = chart.EndDateTime;
            }
            return true;
        }

        /// <summary>
        /// A helper method to check if input schedule manager overlaps any other existing schedules for the agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="scheduledDate"></param>
        /// <param name="agentScheduleManagerCharts"></param>
        /// <returns></returns>
        private async Task<List<string>> CheckOverlapsInExistingSchedules(string agentId, DateTime scheduledDate, List<AgentScheduleManagerChart> agentScheduleManagerCharts)
        {
            List<string> messages = new List<string>();
            var prevDay = scheduledDate.Date.AddDays(-1);
            var nextDay = scheduledDate.Date.AddDays(1);
            var otherDaySchedules = await _agentScheduleManagerRepository.GetManagerSchedules(new List<string>() { agentId }, new List<DateTime>() { prevDay, nextDay });

            foreach(var chart in agentScheduleManagerCharts)
            {
                var dates = Enumerable.Range(0, 1 + (int)(chart.EndDateTime - chart.StartDateTime).TotalMinutes / 5)
                            .Select(offset => chart.StartDateTime.Add(new TimeSpan(0, offset * 5, 0)))
                            .ToArray();

                foreach (var schedule in otherDaySchedules)
                {
                    foreach (var agentManagerChart in schedule.Charts)
                    {
                        var diff = (agentManagerChart.EndDateTime - agentManagerChart.StartDateTime).TotalMinutes;
                        var mgrDates = Enumerable.Range(0, 1 + (int)(agentManagerChart.EndDateTime - agentManagerChart.StartDateTime).TotalMinutes / 5)
                                .Select(offset => agentManagerChart.StartDateTime.Add(new TimeSpan(0, offset * 5, 0)))
                                .ToArray();

                        if (mgrDates.Any(x => dates.Contains(x)))
                        {
                            messages.Add(string.Format(Messages.OverlappingExistingSchedules, chart.StartDateTime.ToString("yyyy-MM-dd HH:mm"), chart.EndDateTime.ToString("yyyy-MM-dd HH:mm"), schedule.Date.ToString("yyyy-MM-dd")));
                        }
                    }
                }
            }

            return messages.Distinct().ToList();
        }

        /// <summary>
        /// A helper method to check if any overlapping schedule exists
        /// </summary>
        /// <param name="calendarChart"></param>
        /// <param name="agentScheduleManagers"></param>
        /// <returns></returns>
        private bool CheckIfOverlapSchedulesExists(CalendarChart calendarChart, List<AgentScheduleManager> agentScheduleManagers)
        {
            var dates = Enumerable.Range(0, 1 + (int) (calendarChart.EndDateTime - calendarChart.StartDateTime).TotalMinutes / 5)
                            .Select(offset => calendarChart.StartDateTime.Add(new TimeSpan(0, offset * 5, 0)))
                            .ToArray();

            foreach (var agentManager in agentScheduleManagers)
            {
                foreach (var agentManagerChart in agentManager.Charts)
                {
                    var diff = (agentManagerChart.EndDateTime - agentManagerChart.StartDateTime).TotalMinutes;
                    var mgrDates = Enumerable.Range(0, 1 + (int) (agentManagerChart.EndDateTime - agentManagerChart.StartDateTime).TotalMinutes / 5)
                            .Select(offset => agentManagerChart.StartDateTime.Add(new TimeSpan(0, offset * 5, 0)))
                            .ToArray();

                    if (mgrDates.Any(x => dates.Contains(x)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// A helper method to check if any overlapping schedule exists
        /// </summary>
        /// <param name="calendarChart"></param>
        /// <param name="otherCalendarCharts"></param>
        /// <returns></returns>
        private bool CheckIfOverlapSchedulesExists(CalendarChart calendarChart, List<CalendarChart> otherCalendarCharts)
        {
            var dates = Enumerable.Range(0, 1 + (int)(calendarChart.EndDateTime - calendarChart.StartDateTime).TotalMinutes / 5)
                            .Select(offset => calendarChart.StartDateTime.Add(new TimeSpan(0, offset * 5, 0)))
                            .ToArray();

            foreach (var otherCalendarChart in otherCalendarCharts)
            {
                var diff = (otherCalendarChart.EndDateTime - otherCalendarChart.StartDateTime).TotalMinutes;
                var otherDates = Enumerable.Range(0, 1 + (int)(otherCalendarChart.EndDateTime - otherCalendarChart.StartDateTime).TotalMinutes / 5)
                        .Select(offset => otherCalendarChart.StartDateTime.Add(new TimeSpan(0, offset * 5, 0)))
                        .ToArray();

                if (otherDates.Any(x => dates.Contains(x)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// A helper method to parse the offset string to a timespan object
        /// </summary>
        /// <param name="offsetString"></param>
        /// <returns>The timespan object</returns>
        private TimeSpan? ParseTimezoneOffset(string offsetString)
        {
            if(offsetString.Trim().Equals("UTC"))
            {
                return new TimeSpan(0, 0, 0);
            }

            TimeSpan? timezoneOffset = null;
            Regex regex = new Regex(@"^UTC(?'offset'(?'hours'(\+|\-)(0|1)[0-9])(?'min'[0-5][0-9]))$");
            var match = regex.Match(offsetString.Trim());
            if(match.Success)
            {
                var matchGroups = match.Groups.Values.ToList();
                var hours = int.Parse(matchGroups.Where(x => x.Name == "hours").Select(x => x.Value).FirstOrDefault());
                var mins = int.Parse(matchGroups.Where(x => x.Name == "min").Select(x => x.Value).FirstOrDefault());
                timezoneOffset = new TimeSpan(hours, mins, 0);
            }
            return timezoneOffset;
        }
        #endregion
    }
}
