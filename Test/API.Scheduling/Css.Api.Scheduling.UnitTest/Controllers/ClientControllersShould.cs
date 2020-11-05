using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.UnitTest.Controllers
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
        ClientsController controller;

        /// <summary>
        /// The service
        /// </summary>
        IClientService service;

        /// <summary>
        /// 
        /// </summary>
        public ClientControllersShould()
        {
            mockClientService = new Mock<IClientService>();
            controller = new ClientsController(mockClientService.Object);
        }

        #region GetClients

        [Fact]
        public async void GetClients()
        {
            ClientQueryParameters clientQueryParameters = new ClientQueryParameters()
            {
                Fields = "",
                OrderBy = "",
                PageNumber = 1,
                PageSize = 10,
                SearchKeyword = ""
            };
            mockClientService.Setup(mr => mr.GetClients(It.IsAny<ClientQueryParameters>())).ReturnsAsync((ClientQueryParameters client) =>
              MockClientService.GetClients(clientQueryParameters));

            var value = await controller.GetClients(clientQueryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetClient

        [Theory]
        [InlineData(2)]
        public async void GetClient_ReturnsOKResult(int clientId)
        {
            mockClientService.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync((ClientIdDetails client) =>
                MockClientService.GetClientOKResult(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.GetClient(clientId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        public async void GetClient_ReturnsNotFoundResult(int clientId)
        {
            mockClientService.Setup(mr => mr.GetClient(It.IsAny<ClientIdDetails>())).ReturnsAsync((ClientIdDetails client) =>
                MockClientService.GetClientNotFoundResult(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.GetClient(clientId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateClient

        [Fact]
        public async void CreateClient()
        {
            // Arrange
            CreateClient clientDetails = new CreateClient()
            {
                Name = "D",
                CreatedBy = "admin",
                RefId=4
            };
            mockClientService.Setup(mr => mr.CreateClient(It.IsAny<CreateClient>())).ReturnsAsync((CreateClient client) =>
                MockClientService.CreateClient(clientDetails));

            var value = await controller.CreateClient(clientDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteClient

        [Theory]
        [InlineData(1)]
        public async void DeleteClient_ReturnsOKResult(int clientId)
        {
            mockClientService.Setup(mr => mr.DeleteClient(It.IsAny<ClientIdDetails>())).ReturnsAsync((ClientIdDetails client) =>
                MockClientService.DeleteClientOKResult(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.DeleteClient(clientId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        public async void DeleteClient_ReturnsNotFoundResult(int clientId)
        {
            mockClientService.Setup(mr => mr.DeleteClient(It.IsAny<ClientIdDetails>())).ReturnsAsync((ClientIdDetails client) =>
                MockClientService.DeleteClientNotFoundResult(new ClientIdDetails { ClientId = clientId }));

            var value = await controller.DeleteClient(clientId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateClient

        [Theory]
        [InlineData(1)]
        public async void UpdateClient_ReturnsOKResult(int clientId)
        {
            UpdateClient updateClient = new UpdateClient()
            {
                Name="X",
                ModifiedBy="admin"
            };

            //mockClientService.Setup(mr => mr.UpdateClient(It.IsAny<ClientIdDetails>(),It.IsAny<UpdateClient>()))    
            //.ReturnsAsync((ClientIdDetails idDetails),(UpdateClient update)=>
            //    MockClientService.UpdateClientOKResult(new ClientIdDetails { ClientId = clientId },updateClient)));

            var value = await controller.DeleteClient(clientId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        public async void UpdateClient_ReturnsNotFoundResult(int clientId)
        {
            UpdateClient updateClient = new UpdateClient()
            {
                Name = "X",
                ModifiedBy = "admin"
            };

            //mockClientService.Setup(mr => mr.UpdateClient(It.IsAny<ClientIdDetails>(), It.IsAny<UpdateClient>()))
            //.ReturnsAsync((ClientIdDetails idDetails),(UpdateClient update) =>
            //    MockClientService.UpdateClientOKResult(new ClientIdDetails { ClientId = clientId }, updateClient)));

            var value = await controller.DeleteClient(clientId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
