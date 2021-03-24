using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.UnitTest.Controllers
{
    public class AgentAdminControllerShould
    {
        /// <summary>
        /// The mock agent admin service
        /// </summary>
        private readonly Mock<IAgentAdminService> mockAgentAdminService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly AgentAdminsController controller;

        /// <summary>
        /// The mock agent admin data
        /// </summary>
        private readonly MockAgentAdminData mockAgentAdminData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentAdminControllerShould" /> class.
        /// </summary>
        public AgentAdminControllerShould()
        {
            mockAgentAdminService = new Mock<IAgentAdminService>();
            mockAgentAdminData = new MockAgentAdminData();
            controller = new AgentAdminsController(mockAgentAdminService.Object);
        }

        ///// <summary>Gets the agent admins.</summary>
        //[Fact]
        //public async void GetAgentAdmins()
        //{
        //    AgentAdminQueryParameter agentAdminQueryParameter = new AgentAdminQueryParameter();

        //    mockAgentAdminService.Setup(mr => mr.GetAgentAdmins(It.IsAny<AgentAdminQueryParameter>())).ReturnsAsync((AgentAdminQueryParameter agentAdmin) =>
        //      mockAgentAdminData.GetAgentAdmins(agentAdminQueryParameter));

        //    var value = await controller.GetAgentAdmins(agentAdminQueryParameter);

        //    Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>Gets the agent admin returns not found result.</summary>
        ///// <param name="agentAdminId">The agent admin identifier.</param>
        //[Theory]
        //[InlineData("6fe0b5ad6a05416894c0718e")]
        //[InlineData("6fe0b5ad6a05416894c0718f")]
        //public async void GetAgentAdmin_ReturnsNotFoundResult(string agentAdminId)
        //{

        //    mockAgentAdminService.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync((AgentAdminIdDetails agentAdminIdDetails) =>
        //       mockAgentAdminData.GetAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId }));

        //    var value = await controller.GetAgentAdmin(agentAdminId);

        //    Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>Gets the agent admin returns ok result.</summary>
        ///// <param name="agentAdminId">The agent admin identifier.</param>
        //[Theory]
        //[InlineData("5fe0b5ad6a05416894c0718d")]
        //[InlineData("5fe0b5c46a05416894c0718f")]
        //public async void GetAgentAdmin_ReturnsOKResult(string agentAdminId)
        //{
        //    mockAgentAdminService.Setup(mr => mr.GetAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync((AgentAdminIdDetails agentAdminIdDetails) =>
        //       mockAgentAdminData.GetAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId }));

        //    var value = await controller.GetAgentAdmin(agentAdminId);

        //    Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        //}



        ///// <summary>Gets the agent admin returns not found result.</summary>
        ///// <param name="agentAdminId">The agent admin identifier.</param>
        //[Theory]
        //[InlineData(100)]
        //[InlineData(101)]
        //public async void GetAgentAdminByEmployeeId_ReturnsNotFoundResult(int employeeId)
        //{
        //    mockAgentAdminService.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync((EmployeeIdDetails employeeIdDetails) =>
        //       mockAgentAdminData.GetAgentAdminByEmployeeId(new EmployeeIdDetails { Id = employeeId }));

        //    var value = await controller.GetAgentAdminByEmployeeId(employeeId);

        //    Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>Gets the agent admin returns ok result.</summary>
        ///// <param name="agentAdminId">The agent admin identifier.</param>
        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //public async void GetAgentAdminByEmployeeId_ReturnsOKResult(int employeeId)
        //{
        //    mockAgentAdminService.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync((EmployeeIdDetails employeeIdDetails) =>
        //       mockAgentAdminData.GetAgentAdminByEmployeeId(new EmployeeIdDetails { Id = employeeId }));

        //    var value = await controller.GetAgentAdminByEmployeeId(employeeId);

        //    Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        //}


        ///// <summary>Creates the agent admin.</summary>
        //[Fact]
        //public async void CreateAgentAdmin()
        //{

        //    CreateAgentAdmin createAgentAdmin = new CreateAgentAdmin();

        //    mockAgentAdminService.Setup(mr => mr.CreateAgentAdmin(It.IsAny<CreateAgentAdmin>())).ReturnsAsync((CreateAgentAdmin createAgentAdmin) =>
        //      mockAgentAdminData.CreateAgentAdmin(createAgentAdmin));

        //    var value = await controller.CreateAgentAdmin(createAgentAdmin);

        //    Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>
        ///// Updates the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agentAdmin identifier.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("6fe0b5ad6a05416894c0718e")]
        //[InlineData("6fe0b5ad6a05416894c0718f")]
        //public async void UpdateAgentAdmin_ReturnsNotFoundResult(string agentAdminId)

        //{
        //    UpdateAgentAdmin updateAgentAdmin = new UpdateAgentAdmin()
        //    {
        //        FirstName = "updated first name",
        //        LastName = "updated last name",
        //        ModifiedBy = "admin"
        //    };

        //    mockAgentAdminService.Setup(mr => mr.UpdateAgentAdmin(It.IsAny<AgentAdminIdDetails>(), It.IsAny<UpdateAgentAdmin>())).ReturnsAsync(
        //        (AgentAdminIdDetails agentAdminIdDetails, UpdateAgentAdmin updateAgentAdmin) =>
        //        mockAgentAdminData.UpdateAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId }, updateAgentAdmin));

        //    var value = await controller.UpdateAgentAdmin(agentAdminId, updateAgentAdmin);

        //    Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>
        ///// Updates the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agentAdmin identifier.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("5fe0b5ad6a05416894c0718d")]
        //[InlineData("5fe0b5c46a05416894c0718f")]
        //public async void UpdateAgentAdmin_ReturnsNoContentResult(string agentAdminId)

        //{
        //    UpdateAgentAdmin updateAgentAdmin = new UpdateAgentAdmin()
        //    {
        //        FirstName = "updated first name",
        //        LastName = "updated last name",
        //        ModifiedBy = "admin"
        //    };

        //    mockAgentAdminService.Setup(mr => mr.UpdateAgentAdmin(It.IsAny<AgentAdminIdDetails>(), It.IsAny<UpdateAgentAdmin>())).ReturnsAsync(
        //        (AgentAdminIdDetails agentAdminIdDetails, UpdateAgentAdmin updateAgentAdmin) =>
        //        mockAgentAdminData.UpdateAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId }, updateAgentAdmin));

        //    var value = await controller.UpdateAgentAdmin(agentAdminId, updateAgentAdmin);

        //    Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);

        //}

        ///// <summary>
        ///// Deletes the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agentAdmin identifier.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("6fe0b5ad6a05416894c0718e")]
        //[InlineData("6fe0b5ad6a05416894c0718f")]
        //public async void DeleteAgentAdmin_ReturnsNotFoundResult(string agentAdminId)
        //{
        //    mockAgentAdminService.Setup(mr => mr.DeleteAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //                   (AgentAdminIdDetails agentAdminIdDetails) =>
        //                   mockAgentAdminData.DeleteAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId }));

        //    var value = await controller.DeleteAgentAdmin(agentAdminId);

        //    Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>
        ///// Deletes the agent admin.
        ///// </summary>
        ///// <param name="agentAdminId">The agentAdmin identifier.</param>
        ///// <returns></returns>
        //[Theory]
        //[InlineData("5fe0b5ad6a05416894c0718d")]
        //[InlineData("5fe0b5c46a05416894c0718f")]
        //public async void DeleteAgentAdmin_ReturnsNoContentResult(string agentAdminId)
        //{
        //    mockAgentAdminService.Setup(mr => mr.DeleteAgentAdmin(It.IsAny<AgentAdminIdDetails>())).ReturnsAsync(
        //                   (AgentAdminIdDetails agentAdminIdDetails) =>
        //                   mockAgentAdminData.DeleteAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId }));

        //    var value = await controller.DeleteAgentAdmin(agentAdminId);

        //    Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>Creates the agent activity logs not found.</summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(30)]
        //[InlineData(31)]
        //public async void CreateAgentActivityLogs_NotFound(int employeeId)
        //{
        //    CreateAgentActivityLog createAgentActivityLog = new CreateAgentActivityLog
        //    {
        //        EmployeeId = employeeId
        //    };

        //    mockAgentAdminService.Setup(mr => mr.CreateAgentActivityLog(It.IsAny<CreateAgentActivityLog>())).ReturnsAsync((CreateAgentActivityLog createAgentActivityLog) =>
        //      mockAgentAdminData.CreateAgentActivityLog(createAgentActivityLog));

        //    var value = await controller.CreateAgentActivityLogs(createAgentActivityLog);

        //    Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>Creates the agent activity logs created.</summary>
        ///// <param name="employeeId">The employee identifier.</param>
        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //public async void CreateAgentActivityLogs_Created(int employeeId)
        //{
        //    CreateAgentActivityLog createAgentActivityLog = new CreateAgentActivityLog
        //    {
        //        EmployeeId = employeeId
        //    };

        //    mockAgentAdminService.Setup(mr => mr.CreateAgentActivityLog(It.IsAny<CreateAgentActivityLog>())).ReturnsAsync((CreateAgentActivityLog createAgentActivityLog) =>
        //      mockAgentAdminData.CreateAgentActivityLog(createAgentActivityLog));

        //    var value = await controller.CreateAgentActivityLogs(createAgentActivityLog);

        //    Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>Gets the agent activity logs.</summary>
        //[Fact]
        //public async void GetAgentActivityLogs()
        //{
        //    ActivityLogQueryParameter activityLogQueryParameter = new ActivityLogQueryParameter();

        //    mockAgentAdminService.Setup(mr => mr.GetAgentActivityLogs(It.IsAny<ActivityLogQueryParameter>())).ReturnsAsync((ActivityLogQueryParameter activityLogQueryParameter) =>
        //      mockAgentAdminData.GetAgentActivityLogs(activityLogQueryParameter));

        //    var value = await controller.GetAgentActivityLogs(activityLogQueryParameter);

        //    Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        //}
    }
}
