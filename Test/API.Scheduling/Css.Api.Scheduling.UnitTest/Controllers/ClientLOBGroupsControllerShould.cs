using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Scheduling.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.UnitTest.Controllers
{
    public class ClientLOBGroupsControllerShould
    {
        /// <summary>
        /// The mock client LOB group service
        /// </summary>
        private readonly Mock<IClientLOBGroupService> mockClientLOBGroupService;

        /// <summary>
        /// The controller
        /// </summary>
        private ClientLOBGroupsController controller;

        /// <summary>
        /// The mock client lob group data
        /// </summary>
        private MockClientLOBGroupData mockClientLOBGroupData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLOBGroupsControllerShould"/> class.
        /// </summary>
        public ClientLOBGroupsControllerShould()
        {
            mockClientLOBGroupService = new Mock<IClientLOBGroupService>();
            mockClientLOBGroupData = new MockClientLOBGroupData();
            controller = new ClientLOBGroupsController(mockClientLOBGroupService.Object);
        }

        #region GetClients

        [Fact]
        public async void GetClientLOBGroups()
        {
            ClientLOBGroupQueryParameter clientLOBGroupQueryParameters = new ClientLOBGroupQueryParameter()
            {
                Fields = "",
                OrderBy = "",
                PageNumber = 1,
                PageSize = 10,
                SearchKeyword = ""
            };

            mockClientLOBGroupService.Setup(mr => mr.GetClientLOBGroups(It.IsAny<ClientLOBGroupQueryParameter>())).ReturnsAsync(
                (ClientLOBGroupQueryParameter client) => mockClientLOBGroupData.GetClientLOBGroups(clientLOBGroupQueryParameters));

            var value = await controller.GetClientLOBGroups(clientLOBGroupQueryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetClientLOBGroup_ReturnsOKResult(int clientLOBGroupId)
        {
            mockClientLOBGroupService.Setup(mr => mr.GetClientLOBGroup(It.IsAny<ClientLOBGroupIdDetails>())).ReturnsAsync((ClientLOBGroupIdDetails client) =>
                mockClientLOBGroupData.GetClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }));

            var value = await controller.GetClientLOBGroup(clientLOBGroupId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetClient_ReturnsNotFoundResult(int clientLOBGroupId)
        {
            mockClientLOBGroupService.Setup(mr => mr.GetClientLOBGroup(It.IsAny<ClientLOBGroupIdDetails>())).ReturnsAsync((ClientLOBGroupIdDetails client) =>
                mockClientLOBGroupData.GetClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }));

            var value = await controller.GetClientLOBGroup(clientLOBGroupId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateClient

        [Fact]
        public async void CreateClient()
        {
            // Arrange
            CreateClientLOBGroup clientLOBGroupDetails = new CreateClientLOBGroup()
            {
                name = "D",
                CreatedBy = "admin",
                RefId = 4
            };
            mockClientLOBGroupService.Setup(mr => mr.CreateClientLOBGroup(It.IsAny<CreateClientLOBGroup>())).ReturnsAsync((CreateClientLOBGroup client) =>
                mockClientLOBGroupData.CreateClientLOBGroup(clientLOBGroupDetails));

            var value = await controller.CreateClientLOBGroup(clientLOBGroupDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateClient_ReturnsNoContentResult(int clientLOBGroupId)
        {
            UpdateClientLOBGroup updateClientLOBGroup = new UpdateClientLOBGroup()
            {
                name = "X",
                ModifiedBy = "admin"
            };

            mockClientLOBGroupService.Setup(mr => mr.UpdateClientLOBGroup(It.IsAny<ClientLOBGroupIdDetails>(), It.IsAny<UpdateClientLOBGroup>())).ReturnsAsync(
                (ClientLOBGroupIdDetails idDetails, UpdateClientLOBGroup update) =>
                mockClientLOBGroupData.UpdateClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }, updateClientLOBGroup));

            var value = await controller.UpdateClientLOBGroup(clientLOBGroupId,updateClientLOBGroup);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateClient_ReturnsNotFoundResult(int clientLOBGroupId)
        {
            UpdateClientLOBGroup updateClientLOBGroup = new UpdateClientLOBGroup()
            {
                name = "X",
                ModifiedBy = "admin"
            };

            mockClientLOBGroupService.Setup(mr => mr.UpdateClientLOBGroup(It.IsAny<ClientLOBGroupIdDetails>(), It.IsAny<UpdateClientLOBGroup>())).ReturnsAsync(
                (ClientLOBGroupIdDetails idDetails, UpdateClientLOBGroup update) =>
                mockClientLOBGroupData.UpdateClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }, updateClientLOBGroup));

            var value = await controller.UpdateClientLOBGroup(clientLOBGroupId,updateClientLOBGroup);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #region DeleteClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteClient_ReturnsNoContentResult(int clientLOBGroupId)
        {
            mockClientLOBGroupService.Setup(mr => mr.DeleteClientLOBGroup(It.IsAny<ClientLOBGroupIdDetails>())).ReturnsAsync((ClientLOBGroupIdDetails client) =>
                mockClientLOBGroupData.DeleteClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }));

            var value = await controller.DeleteClientLOBGroup(clientLOBGroupId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteClient_ReturnsNotFoundResult(int clientLOBGroupId)
        {
            mockClientLOBGroupService.Setup(mr => mr.DeleteClientLOBGroup(It.IsAny<ClientLOBGroupIdDetails>())).ReturnsAsync((ClientLOBGroupIdDetails client) =>
                mockClientLOBGroupData.DeleteClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }));

            var value = await controller.DeleteClientLOBGroup(clientLOBGroupId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #endregion
    }
}
