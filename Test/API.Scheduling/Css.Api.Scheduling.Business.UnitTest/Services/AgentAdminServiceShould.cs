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
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using System.Collections.Generic;
using MongoDB.Bson;

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


        /// <summary>The mock agent schedule manager repository</summary>
        private readonly Mock<IAgentScheduleManagerRepository> mockAgentScheduleManagerRepository;

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

        /// <summary>
        /// The mock timezone repository
        /// </summary>
        private readonly Mock<ITimezoneRepository> mockTimezoneRepository;

        /// <summary>
        /// The mock activity log repository
        /// </summary>
        private readonly Mock<IActivityLogRepository> mockActivityLogRepository;

        /// <summary>
        /// The mock agent category repository
        /// </summary>
        private readonly Mock<IAgentCategoryRepository> mockAgentCategoryRepository;

        /// <summary>
        /// The mock agent scheduling group history repository
        /// </summary>
        private readonly Mock<IAgentSchedulingGroupHistoryRepository> mockAgentSchedulingGroupHistoryRepository;

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
            mockAgentScheduleManagerRepository = new Mock<IAgentScheduleManagerRepository>();
            mockClientRepository = new Mock<IClientRepository>();
            mockClientLobGroupRepository = new Mock<IClientLobGroupRepository>();
            mockSkillGroupRepository = new Mock<ISkillGroupRepository>();
            mockSkillTagRepository = new Mock<ISkillTagRepository>();
            mockAgentSchedulingGroupRepository = new Mock<IAgentSchedulingGroupRepository>();
            mockTimezoneRepository = new Mock<ITimezoneRepository>();
            mockActivityLogRepository = new Mock<IActivityLogRepository>();
            mockAgentCategoryRepository = new Mock<IAgentCategoryRepository>();
            mockAgentSchedulingGroupHistoryRepository = new Mock<IAgentSchedulingGroupHistoryRepository>();
            var mockUnitWork = new Mock<IUnitOfWork>();

            mockDataContext = new MockDataContext();

            agentAdminService = new AgentAdminService(
                mockHttContext.Object,
                mockAgentAdminRepository.Object,
                mockAgentScheduleRepository.Object,
                mockAgentScheduleManagerRepository.Object,
                mockClientRepository.Object,
                mockClientLobGroupRepository.Object,
                mockSkillGroupRepository.Object,
                mockSkillTagRepository.Object,
                mockAgentSchedulingGroupRepository.Object,
                mockTimezoneRepository.Object,
                mockActivityLogRepository.Object,
                mockAgentCategoryRepository.Object,
                mockAgentSchedulingGroupHistoryRepository.Object,
                mapper,
                mockUnitWork.Object
                );
        }

        #region GetAgentAdmins

        /// <summary>
        /// Gets the language translations.
        /// </summary>
        [Fact]
        public async void GetAgentAdmins()
        {
            AgentAdminQueryParameter agentAdminQueryparameter = new AgentAdminQueryParameter();

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmins(It.IsAny<AgentAdminQueryParameter>())).ReturnsAsync(
                (AgentAdminQueryParameter agentAdminQueryparameter) => mockDataContext.GetAgentAdmins(agentAdminQueryparameter));

            var result = await agentAdminService.GetAgentAdmins(agentAdminQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentAdmin

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminId">The agent admin identifier details.</param>
        /// <returns></returns>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0618d")]
        [InlineData("6fe0b5ad6a05416894c0618f")]
        public async void GetAgentAdminWithNotFoundForAdmin(string agentAdminId)
        {
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
                (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
             (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails));

            mockSkillTagRepository.Setup(mr => mr.GetSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
               (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetSkillTag(skillTagIdDetails));

            mockSkillGroupRepository.Setup(mr => mr.GetSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
                (SkillGroupIdDetails skillGroupIdDetails) => mockDataContext.GetSkillGroup(skillGroupIdDetails));

            mockClientLobGroupRepository.Setup(mr => mr.GetClientLobGroup(It.IsAny<ClientLobGroupIdDetails>())).ReturnsAsync(
                (ClientLobGroupIdDetails clientLOBGroupIdDetails) => mockDataContext.GetClientLobGroup(clientLOBGroupIdDetails));

            mockClientRepository.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync(
              (ClientIdDetails clientIdDetails) => mockDataContext.GetClient(clientIdDetails));

            var result = await agentAdminService.GetAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }


        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminId">The agent admin identifier details.</param>
        /// <returns></returns>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718d")]
        [InlineData("5fe0b5c46a05416894c0718f")]
        public async void GetAgentAdmin(string agentAdminId)
        {
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
               (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
             (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails));

            mockSkillTagRepository.Setup(mr => mr.GetSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
               (SkillTagIdDetails skillTagIdDetails) => mockDataContext.GetSkillTag(skillTagIdDetails));

            mockSkillGroupRepository.Setup(mr => mr.GetSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
                (SkillGroupIdDetails skillGroupIdDetails) => mockDataContext.GetSkillGroup(skillGroupIdDetails));

            mockClientLobGroupRepository.Setup(mr => mr.GetClientLobGroup(It.IsAny<ClientLobGroupIdDetails>())).ReturnsAsync(
                (ClientLobGroupIdDetails clientLOBGroupIdDetails) => mockDataContext.GetClientLobGroup(clientLOBGroupIdDetails));

            mockClientRepository.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync(
              (ClientIdDetails clientIdDetails) => mockDataContext.GetClient(clientIdDetails));

            var result = await agentAdminService.GetAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentAdminDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentAdminByEmployeeId

        /// <summary>
        /// Gets the agent admin by employee identifier with not found for admin.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        public async void GetAgentAdminByEmployeeIdWithNotFound(int employeeId)
        {
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            var result = await agentAdminService.GetAgentAdminByEmployeeId(new EmployeeIdDetails { Id = employeeId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent admin by employee identifier.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetAgentAdminByEmployeeId(int employeeId)
        {
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            var result = await agentAdminService.GetAgentAdminByEmployeeId(new EmployeeIdDetails { Id = employeeId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentAdminDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region Add agent admin       

        /// <summary>Creates the agent admin with not found.</summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        public async void CreateAgentAdminNotFound(int agentSchedulingGroupId)
        {
            //var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = employeeId };

            var agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId };


            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeIdAndSso(It.IsAny<EmployeeIdDetails>(), It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
              (EmployeeIdDetails employeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsByEmployeeIdAndSso(employeeIdDetails, agentAdminSsoDetails));
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
           (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
             (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminBySso(agentAdminSsoDetails));

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
          (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails));


            //mockAgentAdminRepository.Setup(mr => mr.CreateAgentAdmin(It.IsAny<Agent>())).ReturnsAsync(
            //          (Agent agent) => mockDataContext.CreateAgentAdmin(agent));

            //mockAgentScheduleRepository.Setup(mr => mr.CreateAgentSchedule(It.IsAny<AgentSchedule>())).ReturnsAsync(
            //          (AgentSchedule agentSchedule) => mockDataContext.CreateAgentSchedule(agentSchedule));

            //mockActivityLogRepository.Setup(mr => mr.CreateActivityLog(It.IsAny<ActivityLog>())).ReturnsAsync(
            //          (ActivityLog activityLog) => mockDataContext.CreateActivityLog(activityLog));

            //mockAgentSchedulingGroupHistoryRepository.Setup(mr => mr.UpdateAgentSchedulingGroupHistory(It.IsAny<AgentSchedulingGroupHistory>())).ReturnsAsync(
            //          (AgentSchedulingGroupHistory agentSchedulingGroupHistory) => mockDataContext.UpdateAgentSchedulingGroupHistory(agentSchedulingGroupHistory));

            CreateAgentAdmin agentDetails = new CreateAgentAdmin
            {
                FirstName = "abc",
                LastName = "def",
                EmployeeId = 51,
                Sso = "user1@concentrix.com",
                AgentSchedulingGroupId = agentSchedulingGroupIdDetails.AgentSchedulingGroupId,
                CreatedBy = "Admin",
                ActivityOrigin = CoreEnums.ActivityOrigin.CSS                
            };

            var result = await agentAdminService.CreateAgentAdmin(agentDetails);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>Creates the agent admin with not found.</summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void CreateAgentAdmin(int agentSchedulingGroupId)
        {
            //var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = employeeId };

            var agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId };


            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeIdAndSso(It.IsAny<EmployeeIdDetails>(), It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
              (EmployeeIdDetails employeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsByEmployeeIdAndSso(employeeIdDetails, agentAdminSsoDetails));
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
           (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
             (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminBySso(agentAdminSsoDetails));

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
          (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails));


            //mockAgentAdminRepository.Setup(mr => mr.CreateAgentAdmin(It.IsAny<Agent>())).ReturnsAsync(
            //          (Agent agent) => mockDataContext.CreateAgentAdmin(agent));

            //mockAgentScheduleRepository.Setup(mr => mr.CreateAgentSchedule(It.IsAny<AgentSchedule>())).ReturnsAsync(
            //          (AgentSchedule agentSchedule) => mockDataContext.CreateAgentSchedule(agentSchedule));

            //mockActivityLogRepository.Setup(mr => mr.CreateActivityLog(It.IsAny<ActivityLog>())).ReturnsAsync(
            //          (ActivityLog activityLog) => mockDataContext.CreateActivityLog(activityLog));

            //mockAgentSchedulingGroupHistoryRepository.Setup(mr => mr.UpdateAgentSchedulingGroupHistory(It.IsAny<AgentSchedulingGroupHistory>())).ReturnsAsync(
            //          (AgentSchedulingGroupHistory agentSchedulingGroupHistory) => mockDataContext.UpdateAgentSchedulingGroupHistory(agentSchedulingGroupHistory));

            CreateAgentAdmin agentDetails = new CreateAgentAdmin
            {
                FirstName = "abc",
                LastName = "def",
                EmployeeId = 51,
                Sso = "user1@concentrix.com",
                AgentSchedulingGroupId = agentSchedulingGroupIdDetails.AgentSchedulingGroupId,
                CreatedBy = "Admin",
                ActivityOrigin = CoreEnums.ActivityOrigin.CSS
            };

            var result = await agentAdminService.CreateAgentAdmin(agentDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentAdminIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateAgentAdmin

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminId">The agent admin identifier details.</param>
        /// <returns></returns>
        [Theory]
        [InlineData("4fe0b5ad6a05416894c0718d")]
        [InlineData("4fe0b5c46a05416894c0718f")]
        public async void UpdateAgentAdminWithNotFound(string agentAdminId)
        {
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
             (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
          (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
             (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminBySso(agentAdminSsoDetails));

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
        (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails));

            //mockAgentAdminRepository.Setup(mr => mr.UpdateAgentAdmin(It.IsAny<Agent>())).ReturnsAsync(
            //          (Agent agent) => mockDataContext.UpdateAgentAdmin(agent));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                      (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentScheduleByEmployeeId(employeeIdDetails));

            //mockAgentScheduleManagerRepository.Setup(mr => mr.UpdateAgentScheduleManagerFromMovingDate(It.IsAny<EmployeeIdDetails>(), It.IsAny<UpdateAgentScheduleManagerEmployeeDetails>())).ReturnsAsync(
            //          (EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleManagerEmployeeDetails updateAgentScheduleManagerEmployeeDetails) => 
            //          mockDataContext.UpdateAgentScheduleManagerFromMovingDate(employeeIdDetails, updateAgentScheduleManagerEmployeeDetails));

            //mockAgentSchedulingGroupHistoryRepository.Setup(mr => mr.UpdateAgentSchedulingGroupHistory(It.IsAny<AgentSchedulingGroupHistory>())).ReturnsAsync(
            //          (AgentSchedulingGroupHistory agentSchedulingGroupHistory) => mockDataContext.UpdateAgentSchedulingGroupHistory(agentSchedulingGroupHistory));

            //mockActivityLogRepository.Setup(mr => mr.UpdateActivityLogsEmployeeId(It.IsAny<EmployeeIdDetails>(), It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
            //          (EmployeeIdDetails employeeIdDetails, EmployeeIdDetails newEmployeeIdDetails) => mockDataContext.UpdateActivityLogsEmployeeId(employeeIdDetails, newEmployeeIdDetails));

            //mockActivityLogRepository.Setup(mr => mr.CreateActivityLog(It.IsAny<ActivityLog>())).ReturnsAsync(
            //          (ActivityLog activityLog) => mockDataContext.CreateActivityLog(activityLog));

            AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };
            UpdateAgentAdmin agentAdmin = new UpdateAgentAdmin
            {
                FirstName = "xyz",
                LastName = "uvw",
                EmployeeId = 1,
                Sso = "user10@concentrix.com",
                AgentSchedulingGroupId = 1,
                ModifiedBy = "admin1",
                ActivityOrigin = CoreEnums.ActivityOrigin.CSS
            };           

            var result = await agentAdminService.UpdateAgentAdmin(agentAdminIdDetails, agentAdmin);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminId">The agent admin identifier details.</param>
        /// <returns></returns>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718d")]
        [InlineData("5fe0b5c46a05416894c0718f")]
        public async void UpdateAgentAdmin(string agentAdminId)
        {
            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
             (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
          (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminBySso(It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
             (AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminBySso(agentAdminSsoDetails));

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
        (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails));

            //mockAgentAdminRepository.Setup(mr => mr.UpdateAgentAdmin(It.IsAny<Agent>())).ReturnsAsync(
            //          (Agent agent) => mockDataContext.UpdateAgentAdmin(agent));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                      (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentScheduleByEmployeeId(employeeIdDetails));

            //mockAgentScheduleManagerRepository.Setup(mr => mr.UpdateAgentScheduleManagerFromMovingDate(It.IsAny<EmployeeIdDetails>(), It.IsAny<UpdateAgentScheduleManagerEmployeeDetails>())).ReturnsAsync(
            //          (EmployeeIdDetails employeeIdDetails, UpdateAgentScheduleManagerEmployeeDetails updateAgentScheduleManagerEmployeeDetails) =>
            //          mockDataContext.UpdateAgentScheduleManagerFromMovingDate(employeeIdDetails, updateAgentScheduleManagerEmployeeDetails));

            //mockAgentSchedulingGroupHistoryRepository.Setup(mr => mr.UpdateAgentSchedulingGroupHistory(It.IsAny<AgentSchedulingGroupHistory>())).ReturnsAsync(
            //          (AgentSchedulingGroupHistory agentSchedulingGroupHistory) => mockDataContext.UpdateAgentSchedulingGroupHistory(agentSchedulingGroupHistory));

            //mockActivityLogRepository.Setup(mr => mr.UpdateActivityLogsEmployeeId(It.IsAny<EmployeeIdDetails>(), It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
            //          (EmployeeIdDetails employeeIdDetails, EmployeeIdDetails newEmployeeIdDetails) => mockDataContext.UpdateActivityLogsEmployeeId(employeeIdDetails, newEmployeeIdDetails));

            //mockActivityLogRepository.Setup(mr => mr.CreateActivityLog(It.IsAny<ActivityLog>())).ReturnsAsync(
            //          (ActivityLog activityLog) => mockDataContext.CreateActivityLog(activityLog));

            AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };
            UpdateAgentAdmin agentAdmin = new UpdateAgentAdmin
            {
                FirstName = "xyz",
                LastName = "uvw",
                EmployeeId = 1,
                Sso = "user10@concentrix.com",
                AgentSchedulingGroupId = 1,
                ModifiedBy = "admin1",
                ActivityOrigin = CoreEnums.ActivityOrigin.CSS
            };

            var result = await agentAdminService.UpdateAgentAdmin(agentAdminIdDetails, agentAdmin);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region Move agent admin

        /// <summary>Moves the agent admins.</summary>
        [Fact]
        public async void MoveAgentAdmins()
        {
            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
        (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminsByIds(It.IsAny<List<ObjectId>>(), It.IsAny<int>())).ReturnsAsync(
             (List<ObjectId> agentAdminIdsDetails, int sourceSchedulingGroupId) => mockDataContext.GetAgentAdminsByIds(agentAdminIdsDetails, sourceSchedulingGroupId));

            MoveAgentAdminsDetails moveAgentAdminsDetails = new MoveAgentAdminsDetails
            {
                AgentAdminIds = new List<string> { "5fe0b5ad6a05416894c0718d", "5fe0b5c46a05416894c0718f" },
                SourceSchedulingGroupId = 1,
                DestinationSchedulingGroupId = 2,
                ModifiedBy = "Admin",
                };

            var result = await agentAdminService.MoveAgentAdmins(moveAgentAdminsDetails);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);

        }
        #endregion

        #region DeleteAgentAdmin
        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        [Theory]
        [InlineData("6fe0b5c46a05416894c0718d")]
        [InlineData("6fe0b5c46a05416894c0718f")]
        public async void DeleteAgentAdminWithNotFound(string agentAdminId)
        {
            AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
                 (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));

            //mockAgentAdminRepository.Setup(mr => mr.UpdateAgentAdmin(It.IsAny<Agent>())).ReturnsAsync(
            //    (Agent agentDetails) => mockDataContext.UpdateAgentAdmin(agentDetails));

            //mockAgentScheduleRepository.Setup(mr => mr.DeleteAgentSchedule(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
            //  (EmployeeIdDetails employeeIdDetails) => mockDataContext.DeleteAgentSchedule(employeeIdDetails));

            var result = await agentAdminService.DeleteAgentAdmin(agentAdminIdDetails);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>Deletes the agent admin.</summary>
        /// <param name="agentAdminId">The agent admin identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718d")]
        [InlineData("5fe0b5c46a05416894c0718f")]
        public async void DeleteAgentAdmin(string agentAdminId)
        {
            AgentAdminIdDetails agentAdminIdDetails = new AgentAdminIdDetails { AgentAdminId = agentAdminId };

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
                 (AgentAdminIdDetails agentAdminIdDetails) => mockDataContext.GetAgentAdmin(agentAdminIdDetails));


            var result = await agentAdminService.DeleteAgentAdmin(agentAdminIdDetails);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region Add activity log        

        /// <summary>Creates the agent activity log with not found.</summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void CreateAgentActivityLog(int employeeId)
        {           
            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = employeeId };

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
               (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            //mockActivityLogRepository.Setup(mr => mr.CreateActivityLog(It.IsAny<ActivityLog>())).ReturnsAsync(
            //   (ActivityLog activityLog) => mockDataContext.CreateActivityLog(activityLog));

            CreateAgentActivityLog agentActivityLogDetails = new CreateAgentActivityLog
            {
                ActivityOrigin = CoreEnums.ActivityOrigin.CSS,
                ActivityStatus = CoreEnums.ActivityStatus.Created,
                EmployeeId = employeeId,
                ExecutedBy = "admin"
            };

            var result = await agentAdminService.CreateAgentActivityLog(agentActivityLogDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }
        #endregion

        #region Get agent activity log

        /// <summary>
        /// Gets the language translations.
        /// </summary>
        [Fact]
        public async void GetAgentActivityLogs()
        {
            ActivityLogQueryParameter activityLogQueryParameter = new ActivityLogQueryParameter();

            mockActivityLogRepository.Setup(mr => mr.GetActivityLogs(It.IsAny<ActivityLogQueryParameter>())).ReturnsAsync(
                (ActivityLogQueryParameter activityLogQueryParameter) => mockDataContext.GetActivityLogs(activityLogQueryParameter));

            var result = await agentAdminService.GetAgentActivityLogs(activityLogQueryParameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }
        #endregion
    }
}