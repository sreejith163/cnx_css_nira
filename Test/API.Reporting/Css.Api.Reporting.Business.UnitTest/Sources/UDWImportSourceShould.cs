using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Sources;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Processing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Sources
{
    /// <summary>
    /// The unit testing class for UDW Source
    /// </summary>
    public class UDWImportSourceShould
    {
        #region Private Properties

        /// <summary>
        /// The source
        /// </summary>
        private readonly ISource _source;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize data
        /// </summary>
        public UDWImportSourceShould()
        {
            var mock = new MockFactoryData();
            _source = new UDWImportSource(mock.GetMockFTP().Object);
        }
        #endregion

        #region Name

        /// <summary>
        /// The method to test the name of the service
        /// </summary>
        [Fact]
        public void CheckName()
        {
            var result = _source.Name;
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }
        #endregion

        #region Pull()

        /// <summary>
        /// The method to test the pull import
        /// </summary>
        [Fact]
        public async void CheckPull()
        {
            var result = await _source.Pull();

            Assert.NotNull(result);
            Assert.IsType<List<DataFeed>>(result);
        }
        #endregion
    }
}
