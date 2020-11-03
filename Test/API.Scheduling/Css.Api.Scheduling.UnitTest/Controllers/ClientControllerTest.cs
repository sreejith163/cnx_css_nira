using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.UnitTest.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Css.Api.Scheduling.UnitTest.Controllers
{
    public class ClientControllerTest
    {

        ClientsController controller;

        IClientService service;

        /// <summary>
        /// 
        /// </summary>
        public ClientControllerTest()
        {
            service = new ClientServiceShould();
            controller = new ClientsController(service);
        }

        #region GetClients

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = controller.GetClients(new ClientQueryParameters
            {
                Fields = "",
                OrderBy = "",
                PageNumber = 1,
                PageSize = 10,
                SearchKeyword = ""
            });
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = controller.GetClients(new ClientQueryParameters
            {
                Fields = "",
                OrderBy = "",
                PageNumber = 1,
                PageSize = 10,
                SearchKeyword = ""
            }).Result as OkObjectResult;
            // Assert
            var items = Assert.IsType<PagedList<Entity>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        #endregion

        #region GetClient

        [Fact]
        public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = controller.GetClient(100);
            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsOkResult()
        {
            // Act
            var okResult = controller.GetClient(1);
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            // Act
            var okResult = controller.GetClient(1).Result as OkObjectResult;
            // Assert
            Assert.IsType<Client>(okResult.Value);
            Assert.Equal(1, (okResult.Value as Client).Id);
        }

        #endregion

        #region CreateClient

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            CreateClient clientDetails = new CreateClient()
            {
                Name = "D",
                CreatedBy = "admin"
            };
            var badResponse = controller.CreateClient(clientDetails);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            CreateClient clientDetails = new CreateClient()
            {
                Name = "D",
                RefId = 4,
                CreatedBy = "admin"
            };
            // Act
            var createdResponse = controller.CreateClient(clientDetails);
            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            CreateClient clientDetails = new CreateClient()
            {
                Name = "D",
                RefId = 4,
                CreatedBy = "admin"
            };
            // Act
            //var createdResponse = controller.CreateClient(clientDetails) as CreatedAtActionResult;
            var item = new Client(); //createdResponse.Value as Client;
            // Assert
            Assert.IsType<Client>(item);
            Assert.Equal("D", item.Name);
        }

        #endregion

        #region DeleteClient

        [Fact]
        public void Remove_NotExistingGuidPassed_ReturnsNotFoundResponse()
        {
            // Act
            var badResponse = controller.DeleteClient(100);
            // Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public void Remove_ExistingGuidPassed_ReturnsOkResult()
        {
            // Act
            var okResponse = controller.DeleteClient(1);
            // Assert
            Assert.IsType<OkResult>(okResponse);
        }

        [Fact]
        public void Remove_ExistingGuidPassed_RemovesOneItem()
        {
            // Act
            var okResponse = controller.DeleteClient(1);
            // Assert
            // Act
            var okResult = controller.GetClients(new ClientQueryParameters
            {
                Fields = "",
                OrderBy = "",
                PageNumber = 1,
                PageSize = 10,
                SearchKeyword = ""
            }).Result as OkObjectResult;
            // Assert
            var items = Assert.IsType<PagedList<Entity>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        #endregion
    }
}
