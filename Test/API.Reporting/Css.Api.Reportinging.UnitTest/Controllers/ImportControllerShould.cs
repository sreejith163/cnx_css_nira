using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Controllers;
using Css.Api.Reporting.Models.DTO.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Css.Api.Reporting.UnitTest.Controllers
{
    /// <summary>
    /// The unit testing class for import controller
    /// </summary>
    public class ImportControllerShould
    {
        #region Private Properties

        /// <summary>
        /// The mock import strategy
        /// </summary>
        private readonly Mock<IImportStrategy> _mockImportStrategy;

        /// <summary>
        /// The import controller
        /// </summary>
        private readonly ImportController _importController;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize mock data
        /// </summary>
        public ImportControllerShould()
        {
            _mockImportStrategy = new Mock<IImportStrategy>();
            _mockImportStrategy.SetReturnsDefault<Task<ImportStrategyResponse>>(Task.FromResult<ImportStrategyResponse>(new ImportStrategyResponse()));
            _importController = new ImportController(_mockImportStrategy.Object);
        }
        #endregion

        #region Post([FromHeader] string importKey)
        
        /// <summary>
        /// The method to test the post call
        /// </summary>
        [Fact]
        public async void CheckPost()
        {
            var response = await _importController.Post("UDW");
            Assert.NotNull(response);
            Assert.IsType<ImportStrategyResponse>(response);
        }
        #endregion
    }
}
