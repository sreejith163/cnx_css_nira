using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Controllers;
using Css.Api.Setup.Models.DTO.Request.Client;
using Css.Api.Setup.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Setup.UnitTest.Controllers
{
    public class ClientControllersShould
    {
        /// <summary>
        /// The mock client service
        /// </summary>
        private readonly Mock<IClientService> mockClientService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly ClientsController controller;

        /// <summary>
        /// The mock client data
        /// </summary>
        private readonly MockClientData mockClientData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientControllersShould" /> class.
        /// </summary>
        public ClientControllersShould()
        {
            mockClientService = new Mock<IClientService>();
            mockClientData = new MockClientData();
            controller = new ClientsController(mockClientService.Object);
        }

        #region GetClients

        /// <summary>
        /// Gets the clients.
        /// </summary>
        [Fact]
        public async void GetClients()
        {
            ClientQueryParameters clientQueryParameters = new ClientQueryParameters();

            mockClientService.Setup(mr => mr.GetClients(It.IsAny<ClientQueryParameters>())).ReturnsAsync((ClientQueryParameters client) =>
              mockClientData.GetClients(clientQueryParameters));

            var value = await controller.GetClients(clientQueryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetClient

        [Theory]
        [InlineData(100)]
        public async void GetClient_ReturnsNotFoundResult(int clientId)
        {
            mockClientService.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync((ClientIdDetails client) =>
                mockClientData.GetClient(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.GetClient(clientId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetClient_ReturnsOKResult(int clientId)
        {
            mockClientService.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync((ClientIdDetails client) =>
                mockClientData.GetClient(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.GetClient(clientId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateClient

        [Fact]
        public async void CreateClient_ReturnsConflictResult()
        {
            CreateClient clientDetails = new CreateClient()
            {
                Name = "B",
                CreatedBy = "admin",
                RefId = 4
            };

            mockClientService.Setup(mr => mr.CreateClient(It.IsAny<CreateClient>())).ReturnsAsync((CreateClient client) =>
                mockClientData.CreateClient(clientDetails));

            var value = await controller.CreateClient(clientDetails);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Fact]
        public async void CreateClient_ReturnsOkResult()
        {
            CreateClient clientDetails = new CreateClient()
            {
                Name = "Z",
                CreatedBy = "admin",
                RefId = 4
            };
            mockClientService.Setup(mr => mr.CreateClient(It.IsAny<CreateClient>())).ReturnsAsync((CreateClient client) =>
                mockClientData.CreateClient(clientDetails));

            var value = await controller.CreateClient(clientDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateClient

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateClient_ReturnsNotFoundResult(int clientId)
        {
            UpdateClient updateClient = new UpdateClient()
            {
                Name = "X",
                ModifiedBy = "admin"
            };

            mockClientService.Setup(mr => mr.UpdateClient(It.IsAny<ClientIdDetails>(), It.IsAny<UpdateClient>())).ReturnsAsync(
                (ClientIdDetails client, UpdateClient update) =>
                mockClientData.UpdateClient(new ClientIdDetails { ClientId = clientId }, updateClient));

            var value = await controller.UpdateClient(clientId, updateClient);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateClient_ReturnsConflictResult(int clientId)
        {
            UpdateClient updateClient = new UpdateClient()
            {
                Name = "C",
                ModifiedBy = "admin"
            };

            mockClientService.Setup(mr => mr.UpdateClient(It.IsAny<ClientIdDetails>(), It.IsAny<UpdateClient>())).ReturnsAsync(
                (ClientIdDetails client, UpdateClient update) =>
                mockClientData.UpdateClient(new ClientIdDetails { ClientId = clientId }, updateClient));

            var value = await controller.UpdateClient(clientId, updateClient);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateClient_ReturnsNoContentResult(int clientId)
        {
            UpdateClient updateClient = new UpdateClient()
            {
                Name="X",
                ModifiedBy="admin"
            };

            mockClientService.Setup(mr => mr.UpdateClient(It.IsAny<ClientIdDetails>(), It.IsAny<UpdateClient>())).ReturnsAsync(
                (ClientIdDetails client, UpdateClient update) =>
                mockClientData.UpdateClient(new ClientIdDetails { ClientId = clientId }, updateClient));

            var value = await controller.UpdateClient(clientId, updateClient);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteClient

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteClient_ReturnsNotFoundResult(int clientId)
        {
            mockClientService.Setup(mr => mr.DeleteClient(It.IsAny<ClientIdDetails>())).ReturnsAsync(
                (ClientIdDetails client) => mockClientData.DeleteClient(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.DeleteClient(clientId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteClient_ReturnsDependencyFailedResult(int clientId)
        {
            mockClientService.Setup(mr => mr.DeleteClient(It.IsAny<ClientIdDetails>())).ReturnsAsync(
                (ClientIdDetails client) => mockClientData.DeleteClient(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.DeleteClient(clientId);

            Assert.Equal((int)HttpStatusCode.FailedDependency, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public async void DeleteClient_ReturnsNoContentResult(int clientId)
        {
            mockClientService.Setup(mr => mr.DeleteClient(It.IsAny<ClientIdDetails>())).ReturnsAsync(
                (ClientIdDetails client) => mockClientData.DeleteClient(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.DeleteClient(clientId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
