using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using CoreEnums = Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Models.Profiles.AgentAdmin;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.Business.UnitTest.Services
{
    public class AgentAdminServiceShould
    {
        /// <summary>
        /// The agent schedule service interface
        /// </summary>
        private readonly IAgentAdminService agentAdminService;

        /// <summary>
        /// The mock agent admin repository
        /// </summary>
        private readonly Mock<IAgentAdminRepository> mockAgentAdminRepository;


        /// <summary>
        /// The mock agent schedule repository
        /// </summary>
        private readonly Mock<IAgentScheduleRepository> mockAgentScheduleRepository;

        /// <summary>
        /// The mock client name repository
        /// </summary>
        private readonly Mock<IClientRepository> mockClientRepository;

        /// <summary>
        /// The mock client lob group repository
        /// </summary>
        private readonly Mock<IClientLobGroupRepository> mockClientLobGroupRepository;

        /// <summary>
        /// The mock skill group repository
        /// </summary>
        private readonly Mock<ISkillGroupRepository> mockSkillGroupRepository;

        /// <summary>
        /// The mock skill tag repository
        /// </summary>
        private readonly Mock<ISkillTagRepository> mockSkillTagRepository;

        /// <summary>
        /// The mock agent scheduling group repository
        /// </summary>
        private readonly Mock<IAgentSchedulingGroupRepository> mockAgentSchedulingGroupRepository;

        /// <summary>The mock activity log repository</summary>
        private readonly Mock<IActivityLogRepository> mockActivityLogRepository;

        /// <summary>
        /// The mock data context
        /// </summary>
        private readonly MockDataContext mockDataContext;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        public AgentAdminServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AgentAdminProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            mockAgentAdminRepository = new Mock<IAgentAdminRepository>();
            mockAgentScheduleRepository = new Mock<IAgentScheduleRepository>();
            mockClientRepository = new Mock<IClientRepository>();
            mockClientLobGroupRepository = new Mock<IClientLobGroupRepository>();
            mockSkillGroupRepository = new Mock<ISkillGroupRepository>();
            mockSkillTagRepository = new Mock<ISkillTagRepository>();
            mockAgentSchedulingGroupRepository = new Mock<IAgentSchedulingGroupRepository>();
            mockActivityLogRepository = new Mock<IActivityLogRepository>();
            var mockUnitWork = new Mock<IUnitOfWork>();

            mockDataContext = new MockDataContext();

            //agentAdminService = new AgentAdminService(
            //    mockHttContext.Object,
            //    mockAgentAdminRepository.Object,
            //    mockAgentScheduleRepository.Object,
            //    mockClientRepository.Object,
            //    mockClientLobGroupRepository.Object,
            //    mockSkillGroupRepository.Object,
            //    mockSkillTagRepository.Object,
            //    mockAgentSchedulingGroupRepository.Object,
            //    mockActivityLogRepository.Object,
            //    mapper,
            //    mockUnitWork.Object
            //    );
        }

        //#region GetAgentAdmins

        ///// <summary>
        ///// Gets the language translations.
        ///// </summary>
        //[Fact]
        //public async void GetAgentAdmins()
        //{
        //    AgentAdminQueryParameter agentAdminQueryparameter = new AgentAdminQueryParameter();

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmins(It.IsAny<AgentAdminQueryParameter>())).ReturnsAsync(
        //        (AgentAdminQueryParameter agentAdminQueryparameter) => mockDataContext.GetAgentAdmins(agentAdminQueryparameter));

        //    var result = await agentAdminService.GetAgentAdmins(agentAdminQueryparameter);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.IsType<PagedList<Entity>>(result.Value);
        //    Assert.Equal(HttpStatusCode.OK, result.Code);
        //}

        //#endregion

        //#region GetAgentAdmin

        ///// <summary>
        ///// Gets the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agent admin identifier details.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("5fe0b5ad6a05416894c0618c")]
        //[InlineData("5fe0b5ad6a05416894c0618d")]
        //public async void GetAgentAdminWithNotFoundForAdmin(string agentAdminId)
        //{
        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //        (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

        //    mockSkillTagRepository.Setup(mr => mr.GetSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
        //       (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetSkillTag(skillTagIdDetails));

        //    mockSkillGroupRepository.Setup(mr => mr.GetSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
        //        (SkillGroupIdDetails skillGroupIdDetails) => mockDataContext.GetSkillGroup(skillGroupIdDetails));

        //    mockClientLobGroupRepository.Setup(mr => mr.GetClientLobGroup(It.IsAny<ClientLobGroupIdDetails>())).ReturnsAsync(
        //        (ClientLobGroupIdDetails clientLOBGroupIdDetails) => mockDataContext.GetClientLobGroup(clientLOBGroupIdDetails));

        //    mockClientRepository.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync(
        //      (ClientIdDetails clientIdDetails) => mockDataContext.GetClient(clientIdDetails));

        //    var result = await agentAdminService.GetAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId });

        //    Assert.NotNull(result);
        //    Assert.Null(result.Value);
        //    Assert.Equal(HttpStatusCode.NotFound, result.Code);
        //}


        ///// <summary>
        ///// Gets the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agent admin identifier details.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("5fe0b5ad6a05416894c0718d")]
        //[InlineData("5fe0b5c46a05416894c0718f")]
        //public async void GetAgentAdmin(string agentAdminId)
        //{
        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //         (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

        //    mockSkillTagRepository.Setup(mr => mr.GetSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
        //        (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetSkillTag(skillTagIdDetails));

        //    mockSkillGroupRepository.Setup(mr => mr.GetSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
        //        (SkillGroupIdDetails skillGroupIdDetails) => mockDataContext.GetSkillGroup(skillGroupIdDetails));

        //    mockClientLobGroupRepository.Setup(mr => mr.GetClientLobGroup(It.IsAny<ClientLobGroupIdDetails>())).ReturnsAsync(
        //        (ClientLobGroupIdDetails clientLOBGroupIdDetails) => mockDataContext.GetClientLobGroup(clientLOBGroupIdDetails));

        //    mockClientRepository.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync(
        //      (ClientIdDetails clientIdDetails) => mockDataContext.GetClient(clientIdDetails));

        //    var result = await agentAdminService.GetAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId });

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.IsType<AgentAdminDetailsDTO>(result.Value);
        //    Assert.Equal(HttpStatusCode.OK, result.Code);
        //}

        //#endregion

        //#region GetAgentAdminByEmployeeId

        ///// <summary>
        ///// Gets the agent admin by employee identifier with not found for admin.
        ///// </summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(100)]
        //[InlineData(101)]
        //public async void GetAgentAdminByEmployeeIdWithNotFoundForAdmin(int employeeId)
        //{
        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //        (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdsByEmployeeId(employeeIdDetails));

        //    var result = await agentAdminService.GetAgentAdminByEmployeeId(new EmployeeIdDetails { Id = employeeId });

        //    Assert.NotNull(result);
        //    Assert.Null(result.Value);
        //    Assert.Equal(HttpStatusCode.NotFound, result.Code);
        //}

        ///// <summary>
        ///// Gets the agent admin by employee identifier.
        ///// </summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //public async void GetAgentAdminByEmployeeId(int employeeId)
        //{
        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //        (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdsByEmployeeId(employeeIdDetails));

        //    var result = await agentAdminService.GetAgentAdminByEmployeeId(new EmployeeIdDetails { Id = employeeId });

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.IsType<AgentAdminDetailsDTO>(result.Value);
        //    Assert.Equal(HttpStatusCode.OK, result.Code);
        //}

        //#endregion

        //#region Add agent admin       

        ///// <summary>Creates the agent admin with not found.</summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(30)]
        //[InlineData(31)]
        //public async void CreateAgentAdminWithNotFound(int employeeId)
        //{
        //    var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = employeeId };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeIdAndSso(It.IsAny<EmployeeIdDetails>(), It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
        //      (EmployeeIdDetails employeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsByEmployeeIdAndSso(employeeIdDetails, agentAdminSsoDetails));
        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //   (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdsByEmployeeId(employeeIdDetails));

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
        //     (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsBySso(agentAdminSsoDetails));

        //    mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroupBasedonSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
        //  (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails));

        //    CreateAgentAdmin agentDetails = new CreateAgentAdmin
        //    {
        //        FirstName = "abc",
        //        LastName = "def",
        //        EmployeeId = 1,
        //        Sso = "user1@concentrix.com",
        //        ClientId = 1,
        //        ClientLobGroupId = 1,
        //        SkillGroupId = 1,
        //        SkillTagId = 1,
        //        CreatedBy = "Admin"
        //    };
        //    //mockAgentAdminRepository.Setup(mr => mr.CreateAgentAdmin(It.IsAny<Agent>())).ReturnsAsync(
        //    //          (Agent agent) => mockDataContext.CreateAgentAdmin(agent));

        //    //mockAgentScheduleRepository.Setup(mr => mr.CreateAgentSchedule(It.IsAny<AgentSchedule>())).ReturnsAsync(
        //    //          (AgentSchedule agentSchedule) => mockDataContext.CreateAgentSchedule(agentSchedule));


        //    var result = await agentAdminService.CreateAgentAdmin(agentDetails);

        //    Assert.NotNull(result);
        //    Assert.Null(result.Value);
        //    Assert.Equal(HttpStatusCode.NotFound, result.Code);
        //}


        ///// <summary>Creates the agent admin.</summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //public async void CreateAgentAdmin(int employeeId)
        //{
        //    var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = employeeId };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeIdAndSso(It.IsAny<EmployeeIdDetails>(), It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
        //      (EmployeeIdDetails employeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsByEmployeeIdAndSso(employeeIdDetails, agentAdminSsoDetails));
        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //   (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdsByEmployeeId(employeeIdDetails));

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
        //     (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsBySso(agentAdminSsoDetails));

        //    mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroupBasedonSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
        //  (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails));

        //    CreateAgentAdmin agentDetails = new CreateAgentAdmin
        //    {
        //        FirstName = "abc",
        //        LastName = "def",
        //        EmployeeId = 1,
        //        Sso = "user1@concentrix.com",
        //        ClientId = 1,
        //        ClientLobGroupId = 1,
        //        SkillGroupId = 1,
        //        SkillTagId = 1,
        //        CreatedBy = "Admin"
        //    };
        //    //mockAgentAdminRepository.Setup(mr => mr.CreateAgentAdmin(It.IsAny<Agent>())).ReturnsAsync(
        //    //          (Agent agent) => mockDataContext.CreateAgentAdmin(agent));

        //    //mockAgentScheduleRepository.Setup(mr => mr.CreateAgentSchedule(It.IsAny<AgentSchedule>())).ReturnsAsync(
        //    //          (AgentSchedule agentSchedule) => mockDataContext.CreateAgentSchedule(agentSchedule));


        //    var result = await agentAdminService.CreateAgentAdmin(agentDetails);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.Equal(HttpStatusCode.Created, result.Code);
        //}

        //#endregion

        //#region UpdateAgentAdmin

        ///// <summary>
        ///// Gets the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agent admin identifier details.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("5fe0b5c46a05416894c0618d")]
        //[InlineData("5fe0b5c46a05416894c0618e")]
        //public async void UpdateAgentAdminWithNotFound(string agentAdminId)
        //{
        //    AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };
        //    UpdateAgentAdmin agentAdmin = new UpdateAgentAdmin
        //    {
        //        FirstName = "abc",
        //        LastName = "def",
        //        EmployeeId = 10,
        //        Sso = "user10@concentrix.com",
        //        ClientId = 1,
        //        ClientLobGroupId = 1,
        //        SkillGroupId = 1,
        //        SkillTagId = 1,
        //        ModifiedBy = "admin"
        //    };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //        (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //        (EmployeeIdDetails EmployeeIdDetaile) => mockDataContext.GetAgentAdminIdsByEmployeeId(EmployeeIdDetaile));

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
        //                    (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsBySso(agentAdminSsoDetails));

        //    mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroupBasedonSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
        //                   (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails));

        //    //mockAgentScheduleRepository.Setup(mr => mr.UpdateAgentSchedule(It.IsAny<EmployeeIdDetails>(), It.IsAny<UpdateAgentScheduleEmployeeDetails>())).ReturnsAsync(
        //    //              (EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails) 
        //    //              => mockDataContext.UpdateAgentSchedule(employeeIdDetails, updateAgentScheduleEmployeeDetails));

        //    var result = await agentAdminService.UpdateAgentAdmin(agentAdminIdDetails, agentAdmin);

        //    Assert.NotNull(result);
        //    Assert.Null(result.Value);
        //    Assert.Equal(HttpStatusCode.NotFound, result.Code);
        //}

        ///// <summary>
        ///// Gets the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agent admin identifier details.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("5fe0b5ad6a05416894c0718d")]
        //[InlineData("5fe0b5c46a05416894c0718f")]
        //public async void UpdateAgentAdmin(string agentAdminId)
        //{
        //    AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };
        //    UpdateAgentAdmin agentAdmin = new UpdateAgentAdmin
        //    {
        //        FirstName = "abc",
        //        LastName = "def",
        //        EmployeeId = 10,
        //        Sso = "user10@concentrix.com",
        //        ClientId = 1,
        //        ClientLobGroupId = 1,
        //        SkillGroupId = 1,
        //        SkillTagId = 1,
        //        ModifiedBy = "admin"
        //    };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //        (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //        (EmployeeIdDetails EmployeeIdDetaile) => mockDataContext.GetAgentAdminIdsByEmployeeId(EmployeeIdDetaile));

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
        //                    (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsBySso(agentAdminSsoDetails));

        //    mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroupBasedonSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
        //                   (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails));

        //    //mockAgentScheduleRepository.Setup(mr => mr.UpdateAgentSchedule(It.IsAny<EmployeeIdDetails>(), It.IsAny<UpdateAgentScheduleEmployeeDetails>())).ReturnsAsync(
        //    //              (EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleEmployeeDetails updateAgentScheduleEmployeeDetails)
        //    //              => mockDataContext.UpdateAgentSchedule(employeeIdDetails, updateAgentScheduleEmployeeDetails));

        //    var result = await agentAdminService.UpdateAgentAdmin(agentAdminIdDetails, agentAdmin);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.Equal(HttpStatusCode.NoContent, result.Code);
        //}

        //#endregion

        //#region DeleteAgentAdmin
        ///// <summary>
        ///// Deletes the agent admin.
        ///// </summary>
        ///// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        //[Theory]
        //[InlineData("5fe0b5c46a05416894c0618d")]
        //[InlineData("5fe0b5c46a05416894c0618e")]
        //public async void DeleteAgentAdminWithNotFound(string agentAdminId)
        //{
        //    AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //         (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));


        //    var result = await agentAdminService.DeleteAgentAdmin(agentAdminIdDetails);

        //    Assert.NotNull(result);
        //    Assert.Null(result.Value);
        //    Assert.Equal(HttpStatusCode.NotFound, result.Code);
        //}

        //[Theory]
        //[InlineData("5fe0b5ad6a05416894c0718d")]
        //[InlineData("5fe0b5c46a05416894c0718f")]
        //public async void DeleteAgentAdmin(string agentAdminId)
        //{
        //    AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //         (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));


        //    var result = await agentAdminService.DeleteAgentAdmin(agentAdminIdDetails);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.Equal(HttpStatusCode.NoContent, result.Code);
        //}

        //#endregion

        //#region Add activity log

        ///// <summary>Creates the agent activity log with not found.</summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(30)]
        //[InlineData(31)]
        //public async void CreateAgentActivityLogWithNotFound(int employeeId)
        //{
        //    CreateAgentActivityLog agentActivityLogDetails = new CreateAgentActivityLog
        //    {
        //        ActivityOrigin = CoreEnums.ActivityOrigin.CSS,
        //        ActivityStatus = CoreEnums.ActivityStatus.Created,
        //        EmployeeId = employeeId,
        //        ExecutedBy = "admin"
        //    };
        //    var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentActivityLogDetails.EmployeeId };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //       (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdsByEmployeeId(employeeIdDetails));

        //    //mockActivityLogRepository.Setup(mr => mr.CreateActivityLogs(It.IsAny<ActivityLog>())).ReturnsAsync(
        //    //   (ActivityLog activityLog) => mockDataContext.CreateActivityLogs(activityLog));

        //    var result = await agentAdminService.CreateAgentActivityLog(agentActivityLogDetails);

        //    Assert.NotNull(result);
        //    Assert.Null(result.Value);
        //    Assert.Equal(HttpStatusCode.NotFound, result.Code);
        //}

        ///// <summary>Creates the agent activity log with not found.</summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //public async void CreateAgentActivityLog(int employeeId)
        //{
        //    CreateAgentActivityLog agentActivityLogDetails = new CreateAgentActivityLog
        //    {
        //        ActivityOrigin = CoreEnums.ActivityOrigin.CSS,
        //        ActivityStatus = CoreEnums.ActivityStatus.Created,
        //        EmployeeId = employeeId,
        //        ExecutedBy = "admin"
        //    };
        //    var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentActivityLogDetails.EmployeeId };

        //    mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
        //       (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdsByEmployeeId(employeeIdDetails));

        //    //mockActivityLogRepository.Setup(mr => mr.CreateActivityLogs(It.IsAny<ActivityLog>())).ReturnsAsync(
        //    //   (ActivityLog activityLog) => mockDataContext.CreateActivityLogs(activityLog));

        //    var result = await agentAdminService.CreateAgentActivityLog(agentActivityLogDetails);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.Equal(HttpStatusCode.Created, result.Code);
        //}
        //#endregion

        //#region Get agent activity log

        ///// <summary>
        ///// Gets the language translations.
        ///// </summary>
        //[Fact]
        //public async void GetAgentActivityLogs()
        //{
        //    ActivityLogQueryParameter activityLogQueryParameter = new ActivityLogQueryParameter();

        //    mockActivityLogRepository.Setup(mr => mr.GetActivityLogs(It.IsAny<ActivityLogQueryParameter>())).ReturnsAsync(
        //        (ActivityLogQueryParameter activityLogQueryParameter) => mockDataContext.GetActivityLogs(activityLogQueryParameter));

        //    var result = await agentAdminService.GetAgentActivityLogs(activityLogQueryParameter);

        //    Assert.NotNull(result);
        //    Assert.NotNull(result.Value);
        //    Assert.IsType<PagedList<Entity>>(result.Value);
        //    Assert.Equal(HttpStatusCode.OK, result.Code);
        //}
        //#endregion
    }
}