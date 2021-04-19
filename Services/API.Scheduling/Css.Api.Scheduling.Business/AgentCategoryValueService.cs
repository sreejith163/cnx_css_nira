using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategory;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.AgentCategoryValueView;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Response.MySchedule;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>
    /// Service for agent category value part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.IAgentCategoryValueService" />
    public class AgentCategoryValueService : IAgentCategoryValueService
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
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentAdminRepository _agentAdminRepository;

        /// <summary>
        /// The agent category value repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>
        /// The agent admin repository
        /// </summary>
        private readonly IAgentCategoryValueRepository _agentCategoryValueRepository;


        /// <summary>
        /// The agent category repository
        /// </summary>
        private readonly IAgentCategoryRepository _agentCategoryRepository;

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

        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryValueService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="activityLogRepository">The activity log repository.</param>
        /// <param name="agentScheduleManagerRepository">The agent schedule manager repository.</param>
        /// <param name="agentAdminRepository">The agent schedule manager repository.</param>
        /// <param name="agentCategoryValueRepository">The agent category value repository.</param>
        ///   /// <param name="agentCategoryRepository">The agent category repository.</param>
        /// <param name="schedulingCodeRepository">The scheduling code repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentCategoryValueService(
            IHttpContextAccessor httpContextAccessor,
            IActivityLogRepository activityLogRepository,
            IAgentScheduleManagerRepository agentScheduleManagerRepository,
            IAgentCategoryValueRepository agentCategoryValueRepository,
            IAgentCategoryRepository agentCategoryRepository,
            IAgentAdminRepository agentAdminRepository,
            ISchedulingCodeRepository schedulingCodeRepository,
            IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            IMapper mapper,
            IUnitOfWork uow,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogRepository = activityLogRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentAdminRepository = agentAdminRepository;
            _agentCategoryValueRepository = agentCategoryValueRepository;
            _agentCategoryRepository = agentCategoryRepository;
            _schedulingCodeRepository = schedulingCodeRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _mapper = mapper;
            _uow = uow;
            _config = configuration;
        }

        /// <summary>
        /// Gets the agent category value.
        /// </summary>
        /// <param name="agentCategoryValueQueryParameter">The agent category value queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentCategoryValues(AgentCategoryValueQueryParameter agentCategoryValueQueryParameter)
        {
            
            var agents = await _agentCategoryValueRepository.GetAgentCategoryValues(agentCategoryValueQueryParameter);

            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agents));

            return new CSSResponse(agents, HttpStatusCode.OK);
        }
       private static class Globals
        {
            // global int
            public static DateTime? dateGlobal;

            // global function
            
        }
        public async Task<CSSResponse> ImportAgentCategoryValue(List<ImportAgentCategoryValue> importAgentCategoryValues)
        {
            int importSuccess = 0;

            List<string> errors = new List<string>();

            ImportResponse importResponse = new ImportResponse();
            var agentProcessList = new List<object>();

            var result = new List<object>();

            var agentCategoryList = _agentCategoryRepository.GetAgentCategoryList();

            var mappedAgentCategoryList = _mapper.Map<List<AgentCategoryGenericValidator>>(agentCategoryList.Result);

            //available agent admin
            var employeeList = _agentAdminRepository.GetAgentList();

            var mappedEmployee = _mapper.Map<List<AgentList>>(employeeList.Result);


            var asgList = _agentSchedulingGroupRepository.GetAgentSchedulingGroupList();

            var mappedAsgList = _mapper.Map<List<AgentSchedulingGroupList>>(asgList.Result);


            foreach (var item in importAgentCategoryValues)
            {
                int index = importAgentCategoryValues.IndexOf(item);
                var startDateValid = DateTime.TryParseExact(item.StartDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDateFormat);
                if (string.IsNullOrWhiteSpace(item.StartDate))
                {
                    Globals.dateGlobal = null;
                }
                else
                {
                    if (!startDateValid)
                    {
                        return new CSSResponse($"Error on line {index + 1} invalid date format", HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        Globals.dateGlobal = DateTime.ParseExact(item.StartDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                }
             
         

             var sd = string.IsNullOrWhiteSpace(item.StartDate) ? (DateTime?)null : DateTime.ParseExact(item.StartDate, "yyyyMMdd", CultureInfo.InvariantCulture);
             
             
               
                if (item.AgentCategory == null || item.AgentCategory == "")
                {
                    return new CSSResponse($"Agent category cannot be null at line {index + 1}", HttpStatusCode.BadRequest);
                }
                else if(item.EmployeeId == null || item.EmployeeId == "")
                {
                    return new CSSResponse($"Employee Id cannot be null at line {index + 1}", HttpStatusCode.BadRequest);
                }
                else if (item.Value == null || item.Value == "")
                {
                    return new CSSResponse($"Value cannot be null at line {index + 1}", HttpStatusCode.BadRequest);
                }
               
              
           
               
                else { 

                var agentCategory = item.AgentCategory.ToLower();

                var categoryId = mappedAgentCategoryList.Where(x => agentCategory.Equals(x.Name.ToLower())).Select(x => x.AgentCategoryId).FirstOrDefault();  

                var dataType = mappedAgentCategoryList.Where(x => agentCategory.Equals(x.Name.ToLower())).Select(x => x.AgentCategoryType).FirstOrDefault();

                var min = mappedAgentCategoryList.Where(x => agentCategory.Equals(x.Name.ToLower())).Select(x => x.DataTypeMinValue).FirstOrDefault();

                var max = mappedAgentCategoryList.Where(x => agentCategory.Equals(x.Name.ToLower())).Select(x => x.DataTypeMaxValue).FirstOrDefault();

                var employeeIdDetails = new EmployeeIdDetails { Id = item.EmployeeId };

                                                             
                if (!mappedAgentCategoryList.Exists(x => x.Name.ToLower() == item.AgentCategory.ToLower()))   
                {
                   

                
                    return new CSSResponse($"Error on line {index + 1} Agent Category is not exist", HttpStatusCode.OK);

                }
              
                else if (!mappedEmployee.Exists(x => x.EmployeeId == item.EmployeeId))
                {
                  
                    return new CSSResponse($"Error on line {index + 1} Employee Id is not exist", HttpStatusCode.OK);
                }
               
                else
                {
                        

                        if (AgentCategoryType.Date == (AgentCategoryType)dataType)
                       {

                        var categoryDateValid = DateTime.TryParseExact(item.Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateFormat);
                        if (!categoryDateValid)
                        {
                         
                            return new CSSResponse($"Error on line {index + 1} invalid date format", HttpStatusCode.BadRequest);
                        }
                        else   // update hire date
                        {

                            var value = dateFormat;
                            var hireDateDictionary = "hire date";
                            if (agentCategory.ToLower().Equals(hireDateDictionary.ToLower()))
                            {
                                var parsedDate = DateTime.Parse(value.ToString("yyyy-MM-dd"));
                                var hireDate = new DateTime(parsedDate.Year, parsedDate.Month, parsedDate.Day, 0, 0, 0, DateTimeKind.Utc);
                                   


                                    var agentCategoryValue = new AgentCategoryValue
                                    {
                                        CategoryId = categoryId,
                                        CategoryValue = value.ToString("yyyy-MM-dd"),
                                        StartDate = Globals.dateGlobal
                                    };
                                _agentAdminRepository.UpdateHireDate(employeeIdDetails, hireDate);
                                _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);
                            } 
                            else //update all category with data type date exept hire date 
                            {
                                var agentCategoryValue = new AgentCategoryValue
                                {
                                    CategoryId = categoryId,
                                    CategoryValue = value.ToString("yyyy-MM-dd"),

                                    StartDate = Globals.dateGlobal
                                };
                                _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);
                            }
                            
                        }
                     
                      }
                      else if(AgentCategoryType.Numeric == (AgentCategoryType)dataType)
                      {
                   
                            int.TryParse(min.Trim(), out int minValue);
                            int.TryParse(max.Trim(), out int maxValue);
                            BigInteger number1;
                            bool succeeded1 = BigInteger.TryParse(item.Value.Trim(), out number1);
                            if (!succeeded1)
                            {
                    

                             return new CSSResponse($"Error on line {index + 1} invalid numeric format", HttpStatusCode.BadRequest);
                            }
                            else if (item.Value.Length < minValue || item.Value.Length > maxValue)
                            {
                        
                                return new CSSResponse($"Error on line {index + 1} invalid numeric length value", HttpStatusCode.BadRequest);

                            }
                            else
                            {
                           
                            var agentCategoryValue = new AgentCategoryValue
                            {
                                CategoryId = categoryId,
                                CategoryValue = item.Value,
                                StartDate = Globals.dateGlobal
                            };
                            _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);
                            }

                        
                      }
                       else if (mappedAsgList.Exists(x => x.Name.ToLower() == item.Value.ToLower())) //check asg if exist and update asg group
                        {
                        var asgFilter = mappedAsgList.Find(x => x.Name.ToLower().Equals(item.Value.ToLower()));
                        var updateAsgRequest = new AgentSchedulingUpdateRequest
                        {
                            AgentSchedulingGroupId = asgFilter.AgentSchedulingGroupId,
                            ClientId = asgFilter.ClientId,
                            ClientLobGroupId = asgFilter.ClientLobGroupId,
                            SkillGroupId = asgFilter.SkillGroupId,
                            SkillTagId = asgFilter.SkillTagId,
                            RefId = asgFilter.RefId

                        };
                        var value = item.Value.Trim();
                        var agentCategoryValue = new AgentCategoryValue
                        {
                            CategoryId = categoryId,
                            CategoryValue = value,
                            StartDate = Globals.dateGlobal
                        };
                        _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);
                        _agentAdminRepository.ImportUpdateAgent(employeeIdDetails, updateAsgRequest);
                        }
                        else if(agentCategory.ToLower().Contains("firstname") || agentCategory.ToLower().Contains("first name")) //update firstname
                        {
                        var firstname = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Value.ToLower());
                        var value = item.Value.Trim();
                        var agentCategoryValue = new AgentCategoryValue
                        {
                            CategoryId = categoryId,
                            CategoryValue = value,
                            StartDate = Globals.dateGlobal
                        };
                        _agentAdminRepository.UpdateFirstName(employeeIdDetails, firstname);
                        _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);
                      }
                      else if (agentCategory.ToLower().Contains("lastname") || agentCategory.ToLower().Contains("last name")) // update lastname
                      {
                        var lastname = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Value.ToLower());
                        var value = item.Value.Trim();
                        var agentCategoryValue = new AgentCategoryValue
                        {
                            CategoryId = categoryId,
                            CategoryValue = value,

                            StartDate = Globals.dateGlobal
                        };
                        _agentAdminRepository.UpdateLastName(employeeIdDetails, lastname);
                        _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);
                     }

                    else if (agentCategory.ToLower().Contains("teamlead") || agentCategory.ToLower().Contains("team lead")) // update lastname
                    {
                        var teamleadName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Value.ToLower());
                        var value = item.Value.Trim();
                        var agentCategoryValue = new AgentCategoryValue
                        {
                            CategoryId = categoryId,
                            CategoryValue = value,

                            StartDate = Globals.dateGlobal
                        };
                        _agentAdminRepository.UpdateTeamLead(employeeIdDetails, teamleadName);
                        _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);
                    }
                    else
                      {
                         if(!mappedAsgList.Exists(x => x.Name.ToLower() == item.Value.ToLower()))
                        {
                          
                                return new CSSResponse($"Error on line {index + 1} agent scheduling group not exist", HttpStatusCode.BadRequest);
                            }

                        else
                        {
                            var value = item.Value.Trim();
                            var agentCategoryValue = new AgentCategoryValue
                            {
                                CategoryId = categoryId,
                                CategoryValue = value,

                                StartDate = Globals.dateGlobal
                            };
                            _agentAdminRepository.ImportUpdateAgentCategoryValue(employeeIdDetails, agentCategoryValue);

                        }
                        }
                    }        
                }
                importSuccess = importSuccess + 1;
            }

            await _uow.Commit();

            return new CSSResponse($"{importSuccess} Data Updated", HttpStatusCode.OK);
        }

        /// <summary>
        /// Processes the agent category values.
        /// </summary>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        private async Task<List<AgentCategoryValueUpdateResponse>> ProcessAgentCategoryValues(List<AgentCategoryDetails> agentCategoryDetails)
        {
            string[] format = { "yyyyMMdd" };
            var response = new List<AgentCategoryValueUpdateResponse>();
            var employeeIds = agentCategoryDetails.Select(x => x.EmployeeId).ToList();
            var agentCategoryIds = agentCategoryDetails.Select(x => x.CategoryId).ToList();

            var agentsByEmployeeIds = await _agentAdminRepository.GetAgentAdminsByEmployeeIds(employeeIds);
            var agentsByCategoryIds = await _agentAdminRepository.GetAgentAdminsByCategoryId(agentCategoryIds);

            foreach (var categoryDetails in agentCategoryDetails)
            {
                var employeeExists = agentsByEmployeeIds.Exists(x => x.Ssn == categoryDetails.EmployeeId);
                var categoryExists = agentsByCategoryIds.Exists(x => x.AgentCategoryValues.Exists(x => x.CategoryId == categoryDetails.CategoryId));
                var categoryTypeValid = IsAgentCatgoryTypeValid(categoryDetails, format);
                var categoryDateValid = DateTime.TryParseExact(categoryDetails.StartDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

                if (Enum.IsDefined(typeof(AgentCategoryType), categoryDetails.CategoryId))
                {
                    var categoryType = (AgentCategoryType)categoryDetails.CategoryId;
                    if (categoryType == AgentCategoryType.Numeric)
                    {
                        var agentCategoryIdDetails = new AgentCategoryIdDetails { AgentCategoryId = categoryDetails.CategoryId };
                        var agentCategory = await _agentCategoryRepository.GetAgentCategory(agentCategoryIdDetails);
                        if (agentCategoryIdDetails != null)
                        {
                            bool isValid = int.TryParse(categoryDetails.CategoryValue.Trim(), out int value);
                            if (isValid)
                            {
                                int.TryParse(agentCategory.DataTypeMinValue.Trim(), out int minValue);
                                int.TryParse(agentCategory.DataTypeMaxValue.Trim(), out int maxValue);
                                if (value <= minValue || value >= maxValue)
                                {
                                    categoryTypeValid = false;
                                }
                            }
                        }
                    }
                }

                if (!employeeExists || !categoryExists || !categoryTypeValid || !categoryDateValid)
                {
                    var agentCategoryValueUpdateResponse = new AgentCategoryValueUpdateResponse
                    {
                        EmployeeId = categoryDetails.EmployeeId
                    };
                    if (!employeeExists)
                    {
                        agentCategoryValueUpdateResponse.Messages.Add("Unrecognized Employee ID");
                    }
                    if (!categoryExists)
                    {
                        agentCategoryValueUpdateResponse.Messages.Add("Unrecognized Agent Category");
                    }
                    if (!categoryTypeValid)
                    {
                        agentCategoryValueUpdateResponse.Messages.Add("Invalid Date Agent Category Type");
                    }
                    if (!categoryDateValid)
                    {
                        agentCategoryValueUpdateResponse.Messages.Add("Invalid Date Format (YYYYMMDD)");
                    }

                    response.Add(agentCategoryValueUpdateResponse);
                }
            }

            return response;
        }
        public class ImportResponse
        {
            public List<string> Errors;
      

        }
        /// <summary>
        /// Determines whether [is agent catgory type valid] [the specified category details].
        /// </summary>
        /// <param name="categoryDetails">The category details.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        ///   <c>true</c> if [is agent catgory type valid] [the specified category details]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAgentCatgoryTypeValid(AgentCategoryDetails categoryDetails, string[] format)
        {
            bool isValid = false;

            if (Enum.IsDefined(typeof(AgentCategoryType), categoryDetails.CategoryId))
            {
                var categoryType = (AgentCategoryType)categoryDetails.CategoryId;
                switch (categoryType)
                {
                    case AgentCategoryType.Numeric:
                        isValid = int.TryParse(categoryDetails.CategoryValue.Trim(), out _);
                        break;

                    case AgentCategoryType.AlphaNumeric:
                        isValid = true;
                        break;

                    case AgentCategoryType.Date:
                        isValid = DateTime.TryParseExact(categoryDetails.CategoryValue.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
                        break;
                }
            }

            return isValid;
        }

    }
}
