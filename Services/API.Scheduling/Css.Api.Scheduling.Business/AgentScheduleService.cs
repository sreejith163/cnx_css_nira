using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>
    /// Service for agent admin part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.AgentScheduleService" />
    public class AgentScheduleService : IAgentScheduleService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The activity log repository
        /// </summary>
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentScheduleRepository _agentScheduleRepository;

        /// <summary>
        /// The agent scheduling group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>
        /// The agent schedule manager repository
        /// </summary>
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The agent admin repository
        /// </summary>
        private readonly IAgentAdminRepository _agentAdminRepository;

        /// <summary>
        /// The scheduling code repository
        /// </summary>
        private readonly ISchedulingCodeRepository _schedulingCodeRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentAdminService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="activityLogRepository">The activity log repository.</param>
        /// <param name="agentScheduleRepository">The agent schedule repository.</param>
        /// <param name="agentScheduleManagerRepository">The agent schedule manager repository.</param>
        /// <param name="agentAdminRepository">The agent admin repository.</param>
        /// <param name="schedulingCodeRepository">The scheduling code repository.</param>
        /// <param name="agentSchedulingGroupRepository">The scheduling group repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentScheduleService(
            IHttpContextAccessor httpContextAccessor,
            IActivityLogRepository activityLogRepository,
            IAgentScheduleRepository agentScheduleRepository, 
            IAgentScheduleManagerRepository agentScheduleManagerRepository,
            IAgentAdminRepository agentAdminRepository,
            IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            ISchedulingCodeRepository schedulingCodeRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogRepository = activityLogRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _agentAdminRepository = agentAdminRepository;
            _schedulingCodeRepository = schedulingCodeRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedules = await _agentScheduleRepository.GetAgentSchedules(agentScheduleQueryparameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentSchedules));

            return new CSSResponse(agentSchedules, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentSchedule = _mapper.Map<AgentScheduleDetailsDTO>(agentSchedule);
            return new CSSResponse(mappedAgentSchedule, HttpStatusCode.OK);
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier details].
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<CSSResponse> IsAgentScheduleRangeExist(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var result = await _agentScheduleRepository.IsAgentScheduleRangeExist(agentScheduleIdDetails, dateRange);
            return new CSSResponse(result, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent schedule charts.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleChartQueryparameter">The agent schedule chart queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentScheduleCharts(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentScheduleChartQueryparameter.FromDate.HasValue && agentScheduleChartQueryparameter.FromDate != default(DateTime))
            {
                var date = agentScheduleChartQueryparameter.FromDate.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedule.Ranges = agentSchedule.Ranges.Where(x => x.DateFrom == dateTimeWithZeroTimeSpan).ToList();
            }

            if (agentScheduleChartQueryparameter.ToDate.HasValue && agentScheduleChartQueryparameter.ToDate != default(DateTime))
            {
                var date = agentScheduleChartQueryparameter.ToDate.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedule.Ranges = agentSchedule.Ranges.Where(x => x.DateTo == dateTimeWithZeroTimeSpan).ToList();
            }

            var mappedAgentScheduleChart = _mapper.Map<AgentScheduleChartDetailsDTO>(agentSchedule);
            return new CSSResponse(mappedAgentScheduleChart, HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates the Agent Admin.
        /// </summary>
        /// <param name="agentScheduleIdDetails"></param>
        /// <param name="agentScheduleDetails"></param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            var dateRange = new DateRange { DateFrom = agentScheduleDetails.DateFrom, DateTo = agentScheduleDetails.DateTo };
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails, dateRange);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }
            
            _agentScheduleRepository.UpdateAgentSchedule(agentScheduleIdDetails, agentScheduleDetails);

            if (agentScheduleDetails.Status == SchedulingStatus.Released)
            {               
                var activityLogs = new List<ActivityLog>();
                var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };
                var agentScheduleRange = await _agentScheduleRepository.GetAgentScheduleRange(agentScheduleIdDetails, dateRange);

                var scheduleManagerCharts = ScheduleHelper.GenerateAgentScheduleManagers(agentSchedule.EmployeeId, agentScheduleRange, agentScheduleDetails.ModifiedBy);

                foreach (var scheduleManagerChart in scheduleManagerCharts)
                {
                    _agentScheduleManagerRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, scheduleManagerChart);

                    var activityLog = GetActivityLogForSchedulingManager(scheduleManagerChart, agentSchedule.EmployeeId,
                                                                         agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                         agentScheduleDetails.ActivityOrigin);
                    activityLogs.Add(activityLog);
                }

                _activityLogRepository.CreateActivityLogs(activityLogs);
            }

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleDetails)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            //var hasValidCodes = await HasValidSchedulingCodes(agentScheduleDetails);
            //if (!hasValidCodes)
            //{
            //    return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            //}

            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month, agentScheduleDetails.DateFrom.Day, 
                                                         0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month, agentScheduleDetails.DateTo.Day, 
                                                       0, 0, 0, DateTimeKind.Utc);

            var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };
            var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

            var agentScheduleRange = agentSchedule.Ranges.FirstOrDefault(x => x.DateFrom == agentScheduleDetails.DateFrom &&
                                                                              x.DateTo == agentScheduleDetails.DateTo);

            if (agentScheduleRange != null && agentScheduleRange.ScheduleCharts.Any())
            {
                foreach (var agentScheduleChart in agentScheduleDetails.AgentScheduleCharts)
                {
                    if (agentScheduleRange.ScheduleCharts.Exists(x => x.Day == agentScheduleChart.Day))
                    {
                        var scheduleChart = agentScheduleRange.ScheduleCharts.Find(x => x.Day == agentScheduleChart.Day);
                        scheduleChart.Charts = agentScheduleChart.Charts;
                    }
                    else
                    {
                        agentScheduleRange.ScheduleCharts.Add(agentScheduleChart);
                    }
                }

                agentScheduleRange.ScheduleCharts = agentScheduleRange.ScheduleCharts.Where(x => x.Charts.Any()).ToList();
                agentScheduleRange.ModifiedBy = agentScheduleDetails.ModifiedBy;
                agentScheduleRange.ModifiedDate = DateTimeOffset.UtcNow;

                _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleRange, modifiedUserDetails);

                var scheduleRange = new AgentScheduleRange {
                    AgentSchedulingGroupId = agentScheduleRange.AgentSchedulingGroupId,
                    DateFrom = agentScheduleRange.DateFrom,
                    DateTo = agentScheduleRange.DateTo,
                    Status = SchedulingStatus.Pending_Schedule,
                    ScheduleCharts = agentScheduleDetails.AgentScheduleCharts,
                    CreatedBy = agentScheduleDetails.ModifiedBy,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                var activityLog = GetActivityLogForSchedulingChart(scheduleRange, agentSchedule.EmployeeId, agentScheduleDetails.ModifiedBy,
                                                                   agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                _activityLogRepository.CreateActivityLog(activityLog);
            }
            else
            {
                var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
                if (agentAdmin != null)
                {
                    agentScheduleDetails.AgentScheduleCharts = agentScheduleDetails.AgentScheduleCharts.Where(x => x.Charts.Any()).ToList();
                    if (agentScheduleDetails.AgentScheduleCharts.Any())
                    {
                        agentScheduleRange = new AgentScheduleRange()
                        {
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            DateFrom = agentScheduleDetails.DateFrom,
                            DateTo = agentScheduleDetails.DateTo,
                            Status = SchedulingStatus.Pending_Schedule,
                            ScheduleCharts = agentScheduleDetails.AgentScheduleCharts,
                            CreatedBy = agentScheduleDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow
                        };

                        _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleRange, modifiedUserDetails);

                        var activityLog = GetActivityLogForSchedulingChart(agentScheduleRange, agentSchedule.EmployeeId, agentScheduleDetails.ModifiedBy,
                                                                           agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                        _activityLogRepository.CreateActivityLog(activityLog);
                    }
                }
            }

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
        
        private DateTime GetWeekFirstDay(DateTime DateToCheck)
        {
            DateTime date = DateToCheck;
            DateTime weekFirstDay = date.AddDays(DayOfWeek.Sunday - date.DayOfWeek);

            return weekFirstDay;
        }


        private DateTime GetWeekLastDay(DateTime FirstDayOfWeek)
        {
            DateTime weekLastDay = FirstDayOfWeek.AddDays(6);

            return weekLastDay;
        }

        private DateRange GetWeekRange(DateTime DateToCheck)
        {
            var firstDayOfWeek = GetWeekFirstDay(DateToCheck);
            var lastDayOfWeek = GetWeekLastDay(firstDayOfWeek);

            firstDayOfWeek = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month,
                                                        firstDayOfWeek.Day, 0, 0, 0, DateTimeKind.Utc);

            lastDayOfWeek = new DateTime(lastDayOfWeek.Year, lastDayOfWeek.Month,
                                            lastDayOfWeek.Day, 0, 0, 0, DateTimeKind.Utc);

            var dateRange = new DateRange { DateFrom = firstDayOfWeek, DateTo = lastDayOfWeek };

            return dateRange;
        }


        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> ImportAgentScheduleChart(AgentScheduleImport agentScheduleImport)
        {

            //TODO: Add validation for scheduling codes

            List<string> errors = new List<string>();
            List<string> success = new List<string>();

            ImportAgentScheduleResponse importAgentScheduleResponse = new ImportAgentScheduleResponse();


            int importCount = 0;
            int importSuccess = 0;
            var activityLogs = new List<ActivityLog>();

            // Get all the list of the unique employeeIds from the imported data
            // This will be used to query the existing schedules of the agents from the imported data
            var employeeIds = agentScheduleImport.AgentScheduleImportData.GroupBy(u => u.EmployeeId).Select(grp => grp.Key);

            // Shape the model by getting and assigning the respective Date Range for the given Start Date
            // Here we used a custom property "Week" as a string to represent the Date Range of the given item
            // This "Week" will be the key to group them by "Date Range" later on.
            var assignRange = agentScheduleImport.AgentScheduleImportData.Select(u =>
            new
            {
                EmployeeId = u.EmployeeId,
                Ranges = new
                {
                    Week = GetWeekRange(u.StartDate).DateFrom.ToString() + ' ' + GetWeekRange(u.StartDate).DateTo.ToString(),
                    Range = new
                    {
                        DateFrom = GetWeekRange(u.StartDate).DateFrom,
                        DateTo = GetWeekRange(u.StartDate).DateTo,
                        ScheduleCharts = new
                        {
                            Day = (int)u.StartDate.DayOfWeek,
                            Charts = new
                            {
                                StartTime = u.StartTime,
                                EndTime = u.EndTime,
                                SchedulingCodeId = u.SchedulingCodeId
                            }
                        }
                    }
                }
            }
            ).ToList();


            // Group the shaped model by Employee Id first
            // Then group the Ranges of each grouped employee id by the "Week" property we made earlier. This will merge all the Schedules with similar Date Range.
            // Then group the ScheduleCharts by Day inside each of the ranges to merge all similar date/day activities.
            var groupByEmployeeId = assignRange.GroupBy(x => x.EmployeeId).Select(u => new
            {
                EmployeeId = u.Key,
                Ranges = u.Select(s => s.Ranges).GroupBy(y => y.Week).Select(z =>
                    new
                    {
                        DateFrom =
                        new DateTime(z.Select(df => df.Range.DateFrom).GroupBy(d => d).Select(e => e.Key).FirstOrDefault().Year, z.Select(df => df.Range.DateFrom).GroupBy(d => d).Select(e => e.Key).FirstOrDefault().Month,
                                                        z.Select(df => df.Range.DateFrom).GroupBy(d => d).Select(e => e.Key).FirstOrDefault().Day, 0, 0, 0, DateTimeKind.Utc),

                        DateTo =
                        new DateTime(z.Select(dt => dt.Range.DateTo).GroupBy(d => d).Select(e => e.Key).FirstOrDefault().Year, z.Select(dt => dt.Range.DateTo).GroupBy(d => d).Select(e => e.Key).FirstOrDefault().Month,
                                                        z.Select(dt => dt.Range.DateTo).GroupBy(d => d).Select(e => e.Key).FirstOrDefault().Day, 0, 0, 0, DateTimeKind.Utc),

                        ScheduleCharts = z.Select(a => a.Range.ScheduleCharts).GroupBy(d => d.Day).Select(sc => new
                        {
                            Day = sc.Key,
                            Charts = sc.Select(c => c.Charts).ToList()
                        }
                        ).ToList()
                    }
                )
            }).ToList();


            // preload all the schedules of all the employees from the imported data
            // use the list of employeeIds that were fetched earlier
            var allAgentSchedulesByEmployeeIdList = await _agentScheduleRepository.GetAgentSchedulesByEmployeeIdList(employeeIds.ToList());

            List<string> errorList = new List<string>();

            foreach (var importSchedule in groupByEmployeeId)
            {
                importCount = importCount + 1;

                var agentSchedulePreUpdate = new List<SchedulingRangeImport>();
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleImport.ModifiedBy };

                // find and check if the employee has an existing schedule inside the list that was fetched earlier
                var agentSchedule = allAgentSchedulesByEmployeeIdList.Find(x => x.EmployeeId == importSchedule.EmployeeId);

                // check if agent schedule exists
                // do nothing if it doesn't exists
                if (agentSchedule != null)
                {

                    var agentRanges = importSchedule.Ranges.ToList();

                    //remove the conflicting schedules
                    var filteredAgentRanges = agentRanges.Where(x => !agentSchedule.Ranges.Any(x2 => x2.Status == SchedulingStatus.Released &&
                                                    ((x.DateFrom < x2.DateTo && x.DateTo > x2.DateFrom) ||
                                                    (x.DateFrom == x2.DateFrom && x.DateTo == x2.DateTo)))
                    ).ToList();


                    foreach (var agentRange in filteredAgentRanges)
                    {

                        var existingScheduleRange = agentSchedule.Ranges
                            .Where(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                    ((agentRange.DateFrom <= x.DateTo && agentRange.DateFrom >= x.DateFrom) ||
                                                    (agentRange.DateTo <= x.DateTo && agentRange.DateTo >= x.DateFrom))).FirstOrDefault();

                        // check if there is an available schedule range
                        // update if existing
                        if (existingScheduleRange != null)
                        {
                            var agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentSchedule.Id.ToString() };

                            var agentScheduleCharts = agentRange.ScheduleCharts.Select(x =>
                                new AgentScheduleChart
                                {
                                    Day = x.Day,
                                    Charts = x.Charts.Select(c => new ScheduleChart
                                    {
                                        EndTime = c.EndTime,
                                        StartTime = c.StartTime,
                                        SchedulingCodeId = c.SchedulingCodeId
                                    }).ToList()
                                }).ToList();

                            // validation for conflicting time range
                            agentScheduleCharts.ForEach(x =>
                            {

                                var insertScheduleCharts = new List<ScheduleChart>();
                                x.Charts.ForEach(a =>
                                {

                                    var checkConflicts = insertScheduleCharts.Any(b =>
                                                ((DateTime.Parse(a.StartTime).TimeOfDay < DateTime.Parse(b.EndTime).TimeOfDay 
                                                && DateTime.Parse(a.EndTime).TimeOfDay > DateTime.Parse(b.StartTime).TimeOfDay) ||
                                                (DateTime.Parse(a.StartTime).TimeOfDay == DateTime.Parse(b.StartTime).TimeOfDay 
                                                || DateTime.Parse(a.EndTime).TimeOfDay == DateTime.Parse(b.EndTime).TimeOfDay)) ||
                                                (DateTime.Parse(a.StartTime).TimeOfDay == DateTime.Parse(b.StartTime).TimeOfDay
                                                && DateTime.Parse(a.EndTime).TimeOfDay > DateTime.Parse(b.EndTime).TimeOfDay) ||
                                                (DateTime.Parse(a.StartTime).TimeOfDay < DateTime.Parse(b.StartTime).TimeOfDay
                                                && DateTime.Parse(a.EndTime).TimeOfDay == DateTime.Parse(b.EndTime).TimeOfDay)
                                                ); 

                                    if (!checkConflicts)
                                    {
                                        insertScheduleCharts.Add(new ScheduleChart
                                        {
                                            StartTime = a.StartTime,
                                            EndTime = a.EndTime,
                                            SchedulingCodeId = a.SchedulingCodeId,
                                        });
                                    }
                                    else
                                    {

                                        //hasError = true;
                                        var date = agentRange.DateFrom.AddDays(x.Day).ToString("yyyy-MM-dd");
                                        errorList.Add($"Duplicate/Invalid Details: Employee ID: {importSchedule.EmployeeId} {a.StartTime} - {a.EndTime} StartDate: {date}");
                                    }

                                });
                                                      
                            });

                            var insertScheduleRange = new AgentScheduleRange
                            {
                                AgentSchedulingGroupId = agentSchedule.ActiveAgentSchedulingGroupId,
                                DateFrom = agentRange.DateFrom,
                                DateTo = agentRange.DateTo,
                                ScheduleCharts = agentScheduleCharts,
                                ModifiedBy = agentScheduleImport.ModifiedBy,
                                Status = SchedulingStatus.Pending_Schedule
                            };

                            var existingDayCharts = existingScheduleRange.ScheduleCharts.Where(x => agentScheduleCharts.Any(s => s.Day == x.Day)).ToList();

                            // replace existing daily charts if the day already exists
                            if (existingDayCharts.Any())
                            {
                                existingDayCharts.ForEach(x => x.Charts = agentScheduleCharts.Find(c => c.Day == x.Day).Charts);
                            }

                            var nonExistingDayCharts = agentScheduleCharts.Where(x => !existingScheduleRange.ScheduleCharts.Exists(c => c.Day == x.Day));

                            // insert daily charts if the day does not have charts yet
                            if (nonExistingDayCharts.Any())
                            {
                                existingScheduleRange.ScheduleCharts.AddRange(nonExistingDayCharts);
                            }


                            //existingScheduleRange.ScheduleCharts = insertScheduleRange.ScheduleCharts;
                            existingScheduleRange.ModifiedBy = agentScheduleImport.ModifiedBy;

                            // update existing range
                            _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, existingScheduleRange, modifiedUserDetails);

                            importSuccess = importSuccess + 1;

                        }
                        else
                        // insert if not existing
                        {
                            var agentScheduleCharts = agentRange.ScheduleCharts.Select(x =>
                                new AgentScheduleChart
                                {
                                    Day = x.Day,
                                    Charts = x.Charts.Select(c => new ScheduleChart
                                    {
                                        EndTime = c.EndTime,
                                        StartTime = c.StartTime,
                                        SchedulingCodeId = c.SchedulingCodeId
                                    }).ToList()
                                }).ToList();

                            // validation for conflicting time range
                            agentScheduleCharts.ForEach(x =>
                            {

                                var insertScheduleCharts = new List<ScheduleChart>();
                                x.Charts.ForEach(a =>
                                {

                                    var checkConflicts = insertScheduleCharts.Any(b =>
                                                ((DateTime.Parse(a.StartTime).TimeOfDay < DateTime.Parse(b.EndTime).TimeOfDay
                                                && DateTime.Parse(a.EndTime).TimeOfDay > DateTime.Parse(b.StartTime).TimeOfDay) ||
                                                (DateTime.Parse(a.StartTime).TimeOfDay == DateTime.Parse(b.StartTime).TimeOfDay
                                                || DateTime.Parse(a.EndTime).TimeOfDay == DateTime.Parse(b.EndTime).TimeOfDay)) ||
                                                (DateTime.Parse(a.StartTime).TimeOfDay == DateTime.Parse(b.StartTime).TimeOfDay
                                                && DateTime.Parse(a.EndTime).TimeOfDay > DateTime.Parse(b.EndTime).TimeOfDay) ||
                                                (DateTime.Parse(a.StartTime).TimeOfDay < DateTime.Parse(b.StartTime).TimeOfDay
                                                && DateTime.Parse(a.EndTime).TimeOfDay == DateTime.Parse(b.EndTime).TimeOfDay)
                                                );

                                    if (!checkConflicts)
                                    {
                                        insertScheduleCharts.Add(new ScheduleChart
                                        {
                                            StartTime = a.StartTime,
                                            EndTime = a.EndTime,
                                            SchedulingCodeId = a.SchedulingCodeId,
                                        });
                                    }
                                    else
                                    {
                                        //hasError = true;
                                        var date = agentRange.DateFrom.AddDays(x.Day).ToString("yyyy-MM-dd");
                                        errorList.Add($"Duplicate/Invalid Details: Employee ID: {importSchedule.EmployeeId} {a.StartTime} - {a.EndTime} StartDate: {date}");
                                    }

                                });

                            });

                            var insertScheduleRange = new AgentScheduleRange
                            {
                                AgentSchedulingGroupId = agentSchedule.ActiveAgentSchedulingGroupId,
                                DateFrom = agentRange.DateFrom,
                                DateTo = agentRange.DateTo,
                                ScheduleCharts = agentScheduleCharts,
                                ModifiedBy = agentScheduleImport.ModifiedBy,
                                Status = SchedulingStatus.Pending_Schedule
                            };

                            var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };

                            //insert the new Agent Schedule Range
                            _agentScheduleRepository.CopyAgentSchedules(employeeIdDetails, insertScheduleRange);

                            importSuccess = importSuccess + 1;
                        }

                    }

                }
            }

            string importedDataCount;
            importedDataCount = $"Successfully imported {importSuccess.ToString()} out of {importCount.ToString()} Schedule Data Rows.";

            //importAgentScheduleResponse.Errors = errors;
            //importAgentScheduleResponse.Success = success;
            importAgentScheduleResponse.ImportStatus = importedDataCount;

            if(errorList.Count > 0)
            {
                // return error if there are conflicting schedules
                return new CSSResponse(errorList, HttpStatusCode.BadRequest);
            }

            await _uow.Commit();

            return new CSSResponse(importAgentScheduleResponse, HttpStatusCode.OK);
        }


        /// <summary>
        /// Copies multiple agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule to copy from.</param>
        /// <param name="agentScheduleDetailsList">The list of agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> MultipleCopyAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, MultipleCopyAgentScheduleRequest agentScheduleDetailsList)
        {
            // get the selected agent schedule first
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);

            var activityLogs = new List<ActivityLog>();

            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            // if no employee ids are passed, this means that all agents were selected from the frontend
            // take the agent scheduling group id and process all the employee ids
            if (!agentScheduleDetailsList.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetailsList.AgentSchedulingGroupId };
                agentScheduleDetailsList.EmployeeIds = await _agentScheduleRepository.GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails);
                agentScheduleDetailsList.EmployeeIds = agentScheduleDetailsList.EmployeeIds.FindAll(x => x != agentSchedule.EmployeeId);
            }

            // convert the source date range to utc
            agentScheduleDetailsList.DateFrom = new DateTime(agentScheduleDetailsList.DateFrom.Year, agentScheduleDetailsList.DateFrom.Month,
                                            agentScheduleDetailsList.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetailsList.DateTo = new DateTime(agentScheduleDetailsList.DateTo.Year, agentScheduleDetailsList.DateTo.Month,
                                                        agentScheduleDetailsList.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            // loop the employee ids
            foreach (var employeeId in agentScheduleDetailsList.EmployeeIds)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetailsList.ModifiedBy };
                var employeeSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);

                if (employeeSchedule != null)
                {
                    // loop the target date ranges
                    foreach (var targetDateRange in agentScheduleDetailsList.SelectedDateRanges)
                    {
                        // convert the target date range to utc
                        targetDateRange.DateFrom = new DateTime(targetDateRange.DateFrom.Year, targetDateRange.DateFrom.Month,
                                                        targetDateRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
                        targetDateRange.DateTo = new DateTime(targetDateRange.DateTo.Year, targetDateRange.DateTo.Month,
                                                                    targetDateRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

                        // check if the target range is released
                        var releasedScheduleRange = employeeSchedule.Ranges.Exists(x => x.Status == SchedulingStatus.Released &&
                                    ((targetDateRange.DateFrom < x.DateTo && targetDateRange.DateTo > x.DateFrom) ||
                                    (targetDateRange.DateFrom == x.DateFrom && targetDateRange.DateTo == x.DateTo)));

                        if (!releasedScheduleRange)
                        {
                            // check for existing pending date range
                            var pendingScheduleRange = employeeSchedule.Ranges
                            .FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                    ((targetDateRange.DateFrom < x.DateTo && targetDateRange.DateTo > x.DateFrom) ||
                                                    (targetDateRange.DateFrom == x.DateFrom && targetDateRange.DateTo == x.DateTo)));

                            // get the copied schedule range and charts
                            var copiedAgentScheduleRange = agentSchedule.Ranges
                                .FirstOrDefault(x => x.DateFrom == agentScheduleDetailsList.DateFrom &&
                                                        x.DateTo == agentScheduleDetailsList.DateTo);

                            if (copiedAgentScheduleRange != null)
                            {
                                // update the pending schedule if existing
                                if (pendingScheduleRange != null)
                                {
                                    if (pendingScheduleRange.ScheduleCharts.Any())
                                    {
                                        // copy and update the charts of the target range from the charts of the source range
                                        pendingScheduleRange.ScheduleCharts = copiedAgentScheduleRange.ScheduleCharts;
                                        pendingScheduleRange.CreatedBy = modifiedUserDetails.ModifiedBy;
                                        pendingScheduleRange.CreatedDate = DateTimeOffset.UtcNow;

                                        var scheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = employeeSchedule.Id.ToString() };

                                        _agentScheduleRepository.UpdateAgentScheduleChart(scheduleIdDetails, pendingScheduleRange, modifiedUserDetails);


                                        var activityLog = GetActivityLogForSchedulingChart(pendingScheduleRange, agentSchedule.EmployeeId,
                                                                                            agentScheduleDetailsList.ModifiedBy, agentScheduleDetailsList.ModifiedUser,
                                                                                            agentScheduleDetailsList.ActivityOrigin);
                                        activityLogs.Add(activityLog);

                                    }

                                }
                                // create the schedule if it does not exist
                                else
                                {
                                    var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
                                    if(agentAdmin != null)
                                    {
                                        // create a new date range
                                        var agentScheduleRange = new AgentScheduleRange
                                        {
                                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                                            DateFrom = targetDateRange.DateFrom,
                                            DateTo = targetDateRange.DateTo,
                                            Status = SchedulingStatus.Pending_Schedule,
                                            ScheduleCharts = copiedAgentScheduleRange.ScheduleCharts,
                                            CreatedBy = modifiedUserDetails.ModifiedBy,
                                            CreatedDate = DateTimeOffset.UtcNow
                                        };

                                        var agentScheduleCharts = agentSchedule.Ranges
                                            .FirstOrDefault(x => x.DateFrom == targetDateRange.DateFrom && x.DateTo == targetDateRange.DateTo)?.ScheduleCharts;

                                        _agentScheduleRepository.MultipleCopyAgentScheduleChart(employeeIdDetails, agentScheduleRange);

                                        var activityLog = GetActivityLogForSchedulingChart(agentScheduleRange, employeeId, agentScheduleDetailsList.ModifiedBy,
                                                                                            agentScheduleDetailsList.ModifiedUser, agentScheduleDetailsList.ActivityOrigin);
                                        activityLogs.Add(activityLog);

                                    }

                                }
                            }
                        }

                    }

                }

            }

            

            _activityLogRepository.CreateActivityLogs(activityLogs);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CopyAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule agentScheduleDetails)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (!agentScheduleDetails.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetails.AgentSchedulingGroupId };
                agentScheduleDetails.EmployeeIds = await _agentScheduleRepository.GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails);
                agentScheduleDetails.EmployeeIds = agentScheduleDetails.EmployeeIds.FindAll(x => x != agentSchedule.EmployeeId);
            }

            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month, 
                                                         agentScheduleDetails.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month, 
                                                       agentScheduleDetails.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var activityLogs = new List<ActivityLog>();

            foreach (var employeeId in agentScheduleDetails.EmployeeIds)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var employeeSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);
                if (employeeSchedule != null)
                {
                    var hasConflictingSchedules = employeeSchedule.Ranges.Exists(x => x.Status == SchedulingStatus.Released &&
                                                                                      ((agentScheduleDetails.DateFrom < x.DateTo && agentScheduleDetails.DateTo > x.DateFrom) ||
                                                                                      (agentScheduleDetails.DateFrom == x.DateFrom && agentScheduleDetails.DateTo == x.DateTo)));

                    if (!hasConflictingSchedules)
                    {
                        var availableScheduleRange = employeeSchedule.Ranges
                                    .FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                         ((agentScheduleDetails.DateFrom < x.DateTo && agentScheduleDetails.DateTo > x.DateFrom) ||
                                                         (agentScheduleDetails.DateFrom == x.DateFrom && agentScheduleDetails.DateTo == x.DateTo)));

                        var copiedAgentScheduleRange = agentSchedule.Ranges
                            .FirstOrDefault(x => x.DateFrom == agentScheduleDetails.DateFrom &&
                                                 x.DateTo == agentScheduleDetails.DateTo);

                        if (copiedAgentScheduleRange != null)
                        {
                            if (availableScheduleRange != null)
                            {
                                if (availableScheduleRange != null && availableScheduleRange.ScheduleCharts.Any())
                                {
                                    availableScheduleRange.ScheduleCharts = copiedAgentScheduleRange.ScheduleCharts;
                                    availableScheduleRange.CreatedBy = modifiedUserDetails.ModifiedBy;
                                    availableScheduleRange.CreatedDate = DateTimeOffset.UtcNow;

                                    var scheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = employeeSchedule.Id.ToString() };
                                    _agentScheduleRepository.UpdateAgentScheduleChart(scheduleIdDetails, availableScheduleRange, modifiedUserDetails);

                                    var activityLog = GetActivityLogForSchedulingChart(availableScheduleRange, agentSchedule.EmployeeId,
                                                                                       agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                                       agentScheduleDetails.ActivityOrigin);
                                    activityLogs.Add(activityLog);
                                }
                            }
                            else
                            {
                                var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
                                if (agentAdmin != null)
                                {
                                    var agentScheduleRange = new AgentScheduleRange
                                    {
                                        AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                                        DateFrom = agentScheduleDetails.DateFrom,
                                        DateTo = agentScheduleDetails.DateTo,
                                        Status = SchedulingStatus.Pending_Schedule,
                                        ScheduleCharts = copiedAgentScheduleRange.ScheduleCharts,
                                        CreatedBy = modifiedUserDetails.ModifiedBy,
                                        CreatedDate = DateTimeOffset.UtcNow
                                    };

                                    var agentScheduleCharts = agentSchedule.Ranges
                                        .FirstOrDefault(x => x.DateFrom == agentScheduleDetails.DateFrom && x.DateTo == agentScheduleDetails.DateTo)?.ScheduleCharts;

                                    _agentScheduleRepository.CopyAgentSchedules(employeeIdDetails, agentScheduleRange);

                                    var activityLog = GetActivityLogForSchedulingChart(agentScheduleRange, employeeId, agentScheduleDetails.ModifiedBy,
                                                                                       agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                                    activityLogs.Add(activityLog);
                                }
                            }
                        }
                    }
                }
            }

            _activityLogRepository.CreateActivityLogs(activityLogs);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRangeDetails">The date range details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRangeDetails)
        {
            var dateRange = new DateRange { DateFrom = dateRangeDetails.OldDateFrom, DateTo = dateRangeDetails.OldDateTo };
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails, dateRange, SchedulingStatus.Pending_Schedule);

            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var newDateFrom = new DateTime(dateRangeDetails.NewDateFrom.Year, dateRangeDetails.NewDateFrom.Month, dateRangeDetails.NewDateFrom.Day,
                                           0, 0, 0, DateTimeKind.Utc);
            var newDateTo = new DateTime(dateRangeDetails.NewDateTo.Year, dateRangeDetails.NewDateTo.Month, dateRangeDetails.NewDateTo.Day,
                                         0, 0, 0, DateTimeKind.Utc);
            var oldDateFrom = new DateTime(dateRangeDetails.OldDateFrom.Year, dateRangeDetails.OldDateFrom.Month, dateRangeDetails.OldDateFrom.Day,
                                           0, 0, 0, DateTimeKind.Utc);
            var oldDateTo = new DateTime(dateRangeDetails.OldDateTo.Year, dateRangeDetails.OldDateTo.Month, dateRangeDetails.OldDateTo.Day,
                                         0, 0, 0, DateTimeKind.Utc);

            var hasConflictingSchedules = agentSchedule.Ranges.Exists(x => oldDateFrom != x.DateFrom && oldDateTo != x.DateTo &&
                                                                           ((dateRangeDetails.NewDateFrom < x.DateTo && dateRangeDetails.NewDateTo > x.DateFrom)) ||
                                                                             dateRangeDetails.NewDateFrom == x.DateFrom && dateRangeDetails.NewDateTo == x.DateTo);
            if (hasConflictingSchedules)
            {
                return new CSSResponse(HttpStatusCode.Conflict);
            }

            _agentScheduleRepository.UpdateAgentScheduleRange(agentScheduleIdDetails, dateRangeDetails);

            var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };
            var activityLogRange = new UpdateActivityLogRange
            {
                DateFrom = oldDateFrom,
                DateTo = oldDateTo,
                NewDateFrom = newDateFrom,
                NewDateTo = newDateTo
            };

            _activityLogRepository.UpdateActivityLogsSchedulingRange(employeeIdDetails, activityLogRange);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var agentScheduleRange = await _agentScheduleRepository.GetAgentScheduleRange(agentScheduleIdDetails, dateRange);
            if (agentScheduleRange == null || agentScheduleRange.Status == SchedulingStatus.Released)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            _agentScheduleRepository.DeleteAgentScheduleRange(agentScheduleIdDetails, dateRange);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }       

        /// <summary>
        /// Gets the activity log for scheduling chart.
        /// </summary>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="executedBy">The executed by.</param>
        /// <param name="executedUser">The executed user.</param>
        /// <param name="activityOrigin">The activity origin.</param>
        /// <returns></returns>
        private ActivityLog GetActivityLogForSchedulingChart(AgentScheduleRange agentScheduleRange, string employeeId, string executedBy, string executedUser,
                                                             ActivityOrigin activityOrigin)
        {
            return new ActivityLog()
            {
                EmployeeId = employeeId,
                ExecutedBy = executedBy,
                ExecutedUser = executedUser,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = activityOrigin,
                ActivityStatus = ActivityStatus.Updated,
                ActivityType = ActivityType.SchedulingGrid,
                SchedulingFieldDetails = new SchedulingFieldDetails() { ActivityLogRange = _mapper.Map<ActivityLogScheduleRange>(agentScheduleRange) }
            };
        }

        /// <summary>
        /// Gets the activity log for scheduling manager.
        /// </summary>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="executedBy">The executed by.</param>
        /// <param name="executedUser">The executed user.</param>
        /// <param name="activityOrigin">The activity origin.</param>
        /// <returns></returns>
        private ActivityLog GetActivityLogForSchedulingManager(AgentScheduleManager agentScheduleManagerChart, string employeeId, string executedBy, string executedUser,
                                                               ActivityOrigin activityOrigin)
        {
            return new ActivityLog()
            {
                EmployeeId = employeeId,
                ExecutedBy = executedBy,
                ExecutedUser = executedUser,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = activityOrigin,
                ActivityStatus = ActivityStatus.Updated,
                ActivityType = ActivityType.SchedulingmanagerGrid,
                SchedulingFieldDetails = new SchedulingFieldDetails() { ActivityLogManager = _mapper.Map<ActivityLogScheduleManager>(agentScheduleManagerChart) }
            };
        }
        
        /// <summary>
        /// Determines whether [has valid scheduling codes] [the specified agent schedule details].
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns>
        ///   <c>true</c> if [has valid scheduling codes] [the specified agent schedule details]; otherwise, <c>false</c>.
        /// </returns>
        private async Task<bool> HasValidSchedulingCodes(object agentScheduleDetails)
        {
            bool isValid = true;
            List<int> codes = new List<int>();

            if (agentScheduleDetails is UpdateAgentScheduleChart)
            {
                var details = agentScheduleDetails as UpdateAgentScheduleChart;
                foreach (var agentScheduleChart in details.AgentScheduleCharts)
                {
                    var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                    codes.AddRange(scheduleCodes);
                }
            }

            codes = codes.Distinct().ToList();

            if (codes.Any())
            {
                var schedulingCodesCount = await _schedulingCodeRepository.GetSchedulingCodesCountByIds(codes);
                if (schedulingCodesCount != codes.Count())
                {
                    isValid = false;
                }
            }

            return isValid;
        }
        public async Task<CSSResponse> AgentSchedulingGroupScheduleExport(int agentSchedulingGroupId)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedulingGroupExport(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var matchSchedulingId = await _schedulingCodeRepository.FindSchedulingCodes();

            var schedCode = (from s in matchSchedulingId
                             select new MatchSchedulingCode
                             {
                                 SchedulingCodeId = s.SchedulingCodeId,
                                 Name = s.Name
                             }
                             ).ToList();
            schedCode.Where(sc => sc.SchedulingCodeId == 23).Select(r => r.Name);

            var msg = new List<object>();

            var items = (from i in agentSchedule
                         from r in i.Ranges
                        
                         from s in r.ScheduleCharts
                         let start_date = r.DateFrom.AddDays(s.Day)

                         let end_date = r.DateFrom.AddDays(s.Day)
                         let check = s.Charts.Zip(s.Charts.Skip(0), (x, y) => y.StartTime == x.EndTime && y.SchedulingCodeId == x.SchedulingCodeId)
                         from c in s.Charts  
                         //end_date.ToString("yyyyMMdd"),
                         select new ExportAgentSchedulingGroupSchedule()
                         {
                             EmployeeId = i.EmployeeId,
                             StartDate = start_date.ToString("yyyyMMdd"),
                             EndDate = end_date.ToString("yyyyMMdd"),
                                       
                             ActivityCode = schedCode
                                            .Where(x => x.SchedulingCodeId == c.SchedulingCodeId)
                                            .Select(x => x.Name).SingleOrDefault(),
                             StartTime = c.StartTime,
                             EndTime = c.EndTime
                         }
                        
                         ).ToList().ToArray();
            //var Items = items.SkipWhile(x => x.EndDate == x.StartTime & x.ActivityCode == x.ActivityCode).Take(1).ToList();


            //var items2 = items.Zip(items.Skip(1), (x, y) => y.StartTime == x.EndTime && y.ActivityCode == x.ActivityCode);

       
            var items2 = items.Select((x, index) => new ExportAgentSchedulingGroupSchedule
                {
                    
                    EmployeeId = x.EmployeeId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ActivityCode = x.ActivityCode,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime

                });

          
                //msg.Add(items2);

            for (int i = 0; i < items.Length; i++)
            {
                if (i < items.Length - 1)
                {

                    var convertStartDate = DateTime.ParseExact(items[i + 1].StartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    var convertEndDate = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    DateTime nextday = DateTime.Parse(convertStartDate);
                    DateTime current = DateTime.Parse(convertEndDate);
                    DateTime nextDay2 = nextday.AddDays(-1);
                    if (items[i].EndTime.Equals(items[i + 1].StartTime) &&
                        items[i].ActivityCode.Equals(items[i + 1].ActivityCode) &&
                        current == nextDay2 &&
                        items[i].EmployeeId == items[i + 1].EmployeeId)
                    {
                        var object1 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = items[i + 1].EndDate,
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i + 1].EndTime
                        };
                        msg.Add(object1);
                        i++;
                    } else if (items[i].EndTime == "12:00 AM" || items[i].EndTime == "12:00 am")
                    {
                       var date_plus_one = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                       DateTime date_increment_one = date_plus_one.AddDays(1);
                        var object2 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = date_increment_one.ToString("yyyyMMdd"),
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i].EndTime
                        };
                        msg.Add(object2);
                        i++;
                    }
                    else
                    {
                        msg.Add(items[i]);
                    }
                }
                else
                {
                    msg.Add(items[i]);
                }
            }
            return new CSSResponse(msg, HttpStatusCode.OK);
        }

        public async Task<CSSResponse> EmployeeScheduleExport(string employeeId)
        {
            var agentSchedule = await _agentScheduleRepository.GetEmployeeScheduleExport(employeeId);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var matchSchedulingId = await _schedulingCodeRepository.FindSchedulingCodes();

            var schedCode = (from s in matchSchedulingId
                             select new MatchSchedulingCode
                             {
                                 SchedulingCodeId = s.SchedulingCodeId,
                                 Name = s.Name
                             }
                             ).ToList();
            //schedCode.Where(sc => sc.SchedulingCodeId == 23).Select(r => r.Name);

            var msg = new List<object>();

            var items = (from i in agentSchedule
                         from r in i.Ranges

                         from s in r.ScheduleCharts
                         let start_date = r.DateFrom.AddDays(s.Day)

                         let end_date = r.DateFrom.AddDays(s.Day)
                         let check = s.Charts.Zip(s.Charts.Skip(0), (x, y) => y.StartTime == x.EndTime && y.SchedulingCodeId == x.SchedulingCodeId)
                         from c in s.Charts
                             //end_date.ToString("yyyyMMdd"),
                         select new ExportAgentSchedulingGroupSchedule()
                         {
                             EmployeeId = i.EmployeeId,
                             StartDate = start_date.ToString("yyyyMMdd"),
                             EndDate = end_date.ToString("yyyyMMdd"),

                             ActivityCode = schedCode
                                            .Where(x => x.SchedulingCodeId == c.SchedulingCodeId)
                                            .Select(x => x.Name).SingleOrDefault(),
                             StartTime = c.StartTime,
                             EndTime = c.EndTime
                         }

                         ).ToList().ToArray();
            //var Items = items.SkipWhile(x => x.EndDate == x.StartTime & x.ActivityCode == x.ActivityCode).Take(1).ToList();


            //var items2 = items.Zip(items.Skip(1), (x, y) => y.StartTime == x.EndTime && y.ActivityCode == x.ActivityCode);


            var items2 = items.Select((x, index) => new ExportAgentSchedulingGroupSchedule
            {

                EmployeeId = x.EmployeeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ActivityCode = x.ActivityCode,
                StartTime = x.StartTime,
                EndTime = x.EndTime

            });


            //msg.Add(items2);

            for (int i = 0; i < items.Length; i++)
            {
                if (i < items.Length - 1)
                {

                    var convertStartDate = DateTime.ParseExact(items[i + 1].StartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    var convertEndDate = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    DateTime nextday = DateTime.Parse(convertStartDate);
                    DateTime current = DateTime.Parse(convertEndDate);
                    DateTime nextDay2 = nextday.AddDays(-1);
                    if (items[i].EndTime.Equals(items[i + 1].StartTime) &&
                        items[i].ActivityCode.Equals(items[i + 1].ActivityCode) &&
                        current == nextDay2 &&
                        items[i].EmployeeId == items[i + 1].EmployeeId)
                    {
                        var object1 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = items[i + 1].EndDate,
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i + 1].EndTime
                        };
                        msg.Add(object1);
                        i++;
                    }
                    else if (items[i].EndTime == "12:00 AM" || items[i].EndTime == "12:00 am")
                    {
                        var date_plus_one = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime date_increment_one = date_plus_one.AddDays(1);
                        var object2 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = date_increment_one.ToString("yyyyMMdd"),
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i].EndTime
                        };
                        msg.Add(object2);
                        i++;
                    }
                    else
                    {
                        msg.Add(items[i]);


                    }
                }
                else
                {
                    msg.Add(items[i]);
                }
            }
            return new CSSResponse(msg, HttpStatusCode.OK);
        }

        public async Task<CSSResponse> GetDateRange(List<int> asgList)
        {
            var date_range = await _agentScheduleRepository.GetDateRange(asgList);
            if(date_range == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }
            var findSchedulingGroup = await _agentSchedulingGroupRepository.FindSchedulingGroup();
            var ranges = date_range.SelectMany(x => x.Ranges.Where(x => x.Status == SchedulingStatus.Pending_Schedule));
            
            var date_range_list =  ranges.Select((i,index) => new 

            {   AgentSchedulingGroupId = i.AgentSchedulingGroupId,
                AgentSchedulingGroup = findSchedulingGroup.Where(x => x.AgentSchedulingGroupId == i.AgentSchedulingGroupId).Select(x => x.Name).SingleOrDefault(),
                DateFrom = i.DateFrom.Date.ToString("yyyy-MM-dd"), 
                DateTo = i.DateTo.Date.ToString("yyyy-MM-dd") 
            }).ToList();

            var distinctRanges = date_range_list.GroupBy(x => new {x.AgentSchedulingGroupId, x.AgentSchedulingGroup, x.DateFrom , x.DateTo} ).Select(y => y.First());
            return new CSSResponse(distinctRanges, HttpStatusCode.OK);
        }

        public async Task<CSSResponse> BatchRelease(BatchRelease batchRelease)
        {
            int releaseSuccess = 0;
            var msg = new List<object>();
         
            foreach (var i in batchRelease.BatchReleaseDetails)
            {
                var releaseRange = new ReleaseRangeDetails { AgentSchedulingGroupId = i.AgentSchedulingGroupId, DateFrom = i.DateFrom, DateTo = i.DateTo };
                
                var getID = await _agentScheduleRepository.GetAgentScheduleIdForRelease(releaseRange);
                
                foreach (var item in getID)
                {
                    var agentScheduleRange = await _agentScheduleRepository.GetAgentScheduleRangeForRelease(releaseRange , item.EmployeeId);
                    _agentScheduleRepository.UpdateAgentScheduleRangeRelease(item.EmployeeId, batchRelease);

                    foreach (var xxx in agentScheduleRange)
                    {
                     
                        var scheduleManagerCharts = ScheduleHelper.GenerateAgentScheduleManagers(item.EmployeeId, xxx, batchRelease.ModifiedBy).Distinct();
                        scheduleManagerCharts.GroupBy(x => (x.AgentSchedulingGroupId, x.Date, x.EmployeeId, x.Charts));
                
                        var employeeIdDetails = new EmployeeIdDetails { Id = item.EmployeeId };
                        var activityLogs = new List<ActivityLog>();
                        foreach (var scheduleManagerChart in scheduleManagerCharts)
                        {
                           
                        _agentScheduleManagerRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, scheduleManagerChart);

                         var activityLog = GetActivityLogForSchedulingManager(scheduleManagerChart, item.EmployeeId,
                                                                        batchRelease.ModifiedBy, batchRelease.ModifiedUser,
                                                                     i.ActivityOrigin);
                        activityLogs.Add(activityLog);
                           msg.Add(scheduleManagerChart);
                         }
                     
                        releaseSuccess = releaseSuccess + 1;
                    }
                }
            }
            await _uow.Commit();
            return new CSSResponse(releaseSuccess, HttpStatusCode.OK);
        }
    }

}
    