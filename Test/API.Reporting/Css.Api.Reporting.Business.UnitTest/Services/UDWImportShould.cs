using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Models.Profiles.Agent;
using Css.Api.Reporting.Models.Profiles.AgentData;
using Css.Api.Reporting.Repository.Interfaces;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Services
{
    /// <summary>
    ///  The unit testing class for UDWImport
    /// </summary>
    public class UDWImportShould
    {
        #region Private Properties

        /// <summary>
        /// The importer
        /// </summary>
        private readonly IImporter _importer;

        /// <summary>
        /// The auto mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The mock xml data instance
        /// </summary>
        private readonly MockXmlData _data;

        /// <summary>
        /// The mock repository
        /// </summary>
        private readonly Mock<IAgentRepository> _mockAgentRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize data
        /// </summary>
        public UDWImportShould()
        {
            _data = new MockXmlData();
            _data.InitializeData();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AgentProfile());
                cfg.AddProfile(new AgentDataProfile());
            });

            _mapper = new Mapper(mapperConfig);
            _mockAgentRepository = new Mock<IAgentRepository>();
            var mockUnitWork = new Mock<IUnitOfWork>();
            
            _importer = new UDWImport(_mapper, _mockAgentRepository.Object, mockUnitWork.Object);
        }
        #endregion

        #region Import(byte[] data)

        /// <summary>
        /// The method to test the successful import
        /// </summary>
        [Fact]
        public async void CheckImportSuccess()
        {
            var data = _data.GetBytes<UDWAgentList>();
            var result = await _importer.Import(data);
            
            Assert.NotNull(result);
            Assert.IsType<ImportResponse>(result);
            Assert.Null(result.Metadata);
            Assert.Equal(result.Status, (int)ProcessStatus.Success);
        }

        /// <summary>
        /// The method to test the partial import
        /// </summary>
        [Fact]
        public async void CheckImportPartial()
        {
            var data = _data.GetBytes<UDWAgentList>((int)ProcessStatus.Partial);
            var result = await _importer.Import(data);

            Assert.NotNull(result);
            Assert.IsType<ImportResponse>(result);
            Assert.NotNull(result.Metadata);
            Assert.Equal(result.Status, (int)ProcessStatus.Partial);
        }

        /// <summary>
        /// The method to test the failed import
        /// </summary>
        [Fact]
        public async void CheckImportFailed()
        {
            var data = _data.GetBytes<UDWAgentList>((int)ProcessStatus.Failed);
            var result = await _importer.Import(data);

            Assert.NotNull(result);
            Assert.IsType<ImportResponse>(result);
            Assert.NotNull(result.Metadata);
            Assert.Equal(result.Status, (int)ProcessStatus.Failed);
        }
        #endregion
    }
}
