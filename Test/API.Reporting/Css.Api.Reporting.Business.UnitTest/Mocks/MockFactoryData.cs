using Css.Api.Reporting.Business.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Css.Api.Reporting.Models.DTO.Mappers;
using Moq;
using Css.Api.Reporting.Models.DTO.Processing;
using System.Threading.Tasks;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;

namespace Css.Api.Reporting.Business.UnitTest.Mocks
{
    /// <summary>
    /// A helper class to generate mock data for all factory related services
    /// </summary>
    public class MockFactoryData
    {
        #region Private Properties
        /// <summary>
        /// The mapper settings - simulating the mapper json
        /// </summary>
        private readonly IOptions<MapperSettings> _mappings;

        /// <summary>
        /// The list of all mock importer objects
        /// </summary>
        private readonly IEnumerable<IImporter> _importers;

        /// <summary>
        /// The list of all mock exporter objects
        /// </summary>
        private readonly IEnumerable<IExporter> _exporters;

        /// <summary>
        /// A mock importer
        /// </summary>
        private readonly Mock<IImporter> _mockImporter;

        /// <summary>
        /// A mock exporter
        /// </summary>
        private readonly Mock<IExporter> _mockExporter;

        /// <summary>
        /// A mock response data feed
        /// </summary>
        private readonly List<DataFeed> _mockFeed;
        #endregion

        #region Constructor
        
        /// <summary>
        /// The constructor to initialize all mocks 
        /// </summary>
        public MockFactoryData()
        {
            _mappings = GenerateMapperOptions();
            _mockFeed = InitializeDataFeed();
            _mockImporter = InitializeMockImporter();
            _mockExporter = InitializeMockExporter();
            _importers = InitializeImporters();
            _exporters = InitializeExporters();
        }
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Method which returns the default mapper settings
        /// </summary>
        /// <returns></returns>
        public IOptions<MapperSettings> GetMapperSettings() => _mappings;

        /// <summary>
        /// Method which returns the mapper settings with individual override data
        /// </summary>
        /// <returns></returns>
        public IOptions<MapperSettings> GetMapperSettingsIndividual() => GenerateMapperOptionsIndividual();

        /// <summary>
        /// Method which returns an empty mapper settings simulation object
        /// </summary>
        /// <returns></returns>
        public IOptions<MapperSettings> GetEmptyMapperSettings() => Options.Create<MapperSettings>(new MapperSettings());

        /// <summary>
        /// Method which returns an instance of DataFeed
        /// </summary>
        /// <returns></returns>
        public DataFeed GetFeed() => _mockFeed.First();

        /// <summary>
        /// Method which returns the list of instances of DataFeed
        /// </summary>
        /// <returns></returns>
        public List<DataFeed> GetFeeds() => _mockFeed;

        /// <summary>
        /// Method which returns an enumerable of all mock importer objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IImporter> GetImporters() => _importers;

        /// <summary>
        /// Method which returns an enumerable of all mock exporter objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IExporter> GetExporters() => _exporters;

        /// <summary>
        /// Method which returns a mock importer object
        /// </summary>
        /// <returns></returns>
        public Mock<IImporter> GetMockImporter() => _mockImporter;

        /// <summary>
        /// Method which returns a mock exporter object
        /// </summary>
        /// <returns></returns>
        public Mock<IExporter> GetMockExporter() => _mockExporter;

        /// <summary>
        /// Method which returns a mock importer object which has mock responses configured based on the input status
        /// </summary>
        /// <param name="status">The process status</param>
        /// <returns></returns>
        public Mock<IImporter> GetMockImporter(int status) => InitializeMockImporter(status);

        /// <summary>
        /// Method which returns a mock exporter object which has mock responses configured based on the input status
        /// </summary>
        /// <param name="status">The process status</param>
        /// <returns></returns>
        public Mock<IExporter> GetMockExporter(int status) => InitializeMockExporter(status);
        #endregion

        #region Private Methods

        /// <summary>
        /// A method to generate the mock IOptions for MapperSettings
        /// </summary>
        /// <returns></returns>
        private IOptions<MapperSettings> GenerateMapperOptions()
        {
            return Options.Create<MapperSettings>(new MapperSettings()
            {
                GlobalSettings = new MapperGlobalSettings()
                {
                    FTPServer = "sftp://username:pwd@0.0.0.0",
                    FTPInbox = "/ftp/{0}/Inbox",
                    FTPOutbox = "/ftp/{0}/Outbox"
                },
                Imports = new List<MapperIndividualSettings>()
                {
                    new MapperIndividualSettings()
                    {
                        Key = "UDW",
                        Service = "UDWImporter"
                    },
                    new MapperIndividualSettings()
                    {
                        Key = "eStart",
                        Service = "eStartImporter"
                    },
                    new MapperIndividualSettings()
                    {
                        Key = "Test"
                    }
                },
                Exports = new List<MapperIndividualSettings>()
                {
                    new MapperIndividualSettings()
                    {
                        Key = "eStart",
                        Service = "eStartExporter"
                    },
                    new MapperIndividualSettings()
                    {
                        Key = "Test"
                    }
                }
            });

        }

        /// <summary>
        /// A method to generate the mock IOptions for MapperSettings with individual configuration overrides
        /// </summary>
        /// <returns></returns>
        private IOptions<MapperSettings> GenerateMapperOptionsIndividual()
        {
            return Options.Create<MapperSettings>(new MapperSettings()
            {
                GlobalSettings = new MapperGlobalSettings()
                {
                    FTPServer = "",
                    FTPInbox = "",
                    FTPOutbox = ""
                },
                Imports = new List<MapperIndividualSettings>()
                {
                    new MapperIndividualSettings()
                    {
                        Key = "UDW",
                        Service = "UDWImporter",
                        FTPServer = "sftp://username:pwd@0.0.0.0",
                        FTPFolder = "/ftp/UDW/Inbox"
                    },
                    new MapperIndividualSettings()
                    {
                        Key = "eStart",
                        Service = "eStartImporter",
                        FTPServer = "sftp://username:pwd@0.0.0.0",
                        FTPFolder = "/ftp/eStart/Inbox"
                    }
                },
                Exports = new List<MapperIndividualSettings>()
                {
                    new MapperIndividualSettings()
                    {
                        Key = "eStart",
                        Service = "eStartExporter",
                        FTPFolder = "sftp://username:pwd@0.0.0.0",
                        FTPServer = "/ftp/eStart/Outbox"
                    }
                }
            });

        }

        /// <summary>
        /// A method to generate a mock list of instance of DataFeed
        /// </summary>
        /// <returns></returns>
        private List<DataFeed> InitializeDataFeed()
        {
            return new List<DataFeed>()
            {
                new DataFeed
                {
                    Accessor = "test",
                    Content = new byte[] { 10, 20, 30 },
                    Path = "/ftp/path"
                }
            };
        }

        /// <summary>
        /// A method to initialize a mock importer and customize the responses based on the input status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private Mock<IImporter> InitializeMockImporter(int status = (int)ProcessStatus.Success)
        {
            var importer = new Mock<IImporter>();
            importer.SetReturnsDefault<string>("UDWImporter");
            importer.SetReturnsDefault<Task<ImportResponse>>(Task.FromResult<ImportResponse>(new ImportResponse { 
                Status = status
            }));
            return importer;
        }

        /// <summary>
        /// A method to initialize a mock exporter and customize the responses based on the input status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private Mock<IExporter> InitializeMockExporter(int status = (int)ProcessStatus.Success)
        {
            var exporter = new Mock<IExporter>();
            exporter.SetReturnsDefault<string>("eStartExporter");
            exporter.SetReturnsDefault<Task<ExportResponse>>(Task.FromResult<ExportResponse>(new ExportResponse
            {
                
            }));
            return exporter;
        }

        /// <summary>
        /// A method to generate and initialize an enumerable of importer mock objects
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IImporter> InitializeImporters()
        {
            IList<IImporter> importers = new List<IImporter>();
            var udw = new Mock<IImporter>();
            udw.SetReturnsDefault<string>("UDWImporter");

            importers.Add(udw.Object);

            
            var eStart = new Mock<IImporter>();
            eStart.SetReturnsDefault<string>("eStartImporter");

            importers.Add(eStart.Object);

            return importers;
        }

        /// <summary>
        /// A method to generate and initialize an enumerable of exporter mock objects
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IExporter> InitializeExporters()
        {
            IList<IExporter> exporters = new List<IExporter>();
            var udw = new Mock<IExporter>();
            udw.SetReturnsDefault<string>("UDWExporter");

            exporters.Add(udw.Object);


            var eStart = new Mock<IExporter>();
            eStart.SetReturnsDefault<string>("eStartExporter");

            exporters.Add(eStart.Object);

            return exporters;
        }
        #endregion

    }
}
