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
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Models.DTO.Request;

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
        /// A mock IMapperService
        /// </summary>
        private readonly Mock<IMapperService> _mapperService;

        /// <summary>
        /// The list of all mock source objects
        /// </summary>
        private readonly IEnumerable<ISource> _sources;

        /// <summary>
        /// The list of all mock target objects
        /// </summary>
        private readonly IEnumerable<ITarget> _targets;

        /// <summary>
        /// A mock importer
        /// </summary>
        private readonly Mock<ISource> _mockSource;

        /// <summary>
        /// A mock exporter
        /// </summary>
        private readonly Mock<ITarget> _mockTarget;

        /// <summary>
        /// A mock ftp service
        /// </summary>
        private readonly Mock<IFTPService> _mockFtp;

        /// <summary>
        /// A mock response data feed
        /// </summary>
        private readonly List<DataFeed> _mockFeed;

        /// <summary>
        /// A mock mapping context
        /// </summary>
        private readonly MappingContext _mockContext;

        /// <summary>
        /// A list of mock mapping contexts
        /// </summary>
        private readonly List<MappingContext> _contexts;
        #endregion

        #region Constructor
        
        /// <summary>
        /// The constructor to initialize all mocks 
        /// </summary>
        public MockFactoryData()
        {
            _mappings = GenerateMapperOptions();
            _mockFeed = InitializeDataFeed();
            _mockSource = InitializeMockSource();
            _mockTarget = InitializeMockTarget();
            _sources = InitializeSources();
            _targets = InitializeTargets();
            _mapperService = InitializeMockMapper();
            _mockFtp = InitializeMockFTP();
            _mockContext = InitializeMappingContext();
            _contexts = GenerateMappingContexts();
        }
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Method which returns the default mapper settings
        /// </summary>
        /// <returns></returns>
        public IOptions<MapperSettings> GetMapperSettings() => _mappings;

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
        /// Method which returns an instances of MappingContext
        /// </summary>
        /// <returns></returns>
        public MappingContext GetMappingContext() => _mockContext;

        public List<MappingContext> GetMappingContexts() => _contexts;

        /// <summary>
        /// Method which returns a mock mapper service
        /// </summary>
        /// <returns></returns>
        public Mock<IMapperService> GetMockMapper() => _mapperService; 
        
        /// <summary>
        /// Method which returns an enumerable of all mock importer objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ISource> GetSources() => _sources;

        /// <summary>
        /// Method which returns an enumerable of all mock exporter objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITarget> GetTargets() => _targets;

        /// <summary>
        /// Method which returns a mock ftp service
        /// </summary>
        /// <returns></returns>
        public Mock<IFTPService> GetMockFTP() => _mockFtp;

        /// <summary>
        /// Method which returns a mock importer object
        /// </summary>
        /// <returns></returns>
        public Mock<ISource> GetMockSource() => _mockSource;

        /// <summary>
        /// Method which returns a mock exporter object
        /// </summary>
        /// <returns></returns>
        public Mock<ITarget> GetMockTarget() => _mockTarget;

        /// <summary>
        /// Method which returns a mock importer object which has mock responses configured based on the input status
        /// </summary>
        /// <param name="status">The process status</param>
        /// <returns></returns>
        public Mock<ISource> GetMockSource(int status) => InitializeMockSource(status);

        /// <summary>
        /// Method which returns a mock exporter object which has mock responses configured based on the input status
        /// </summary>
        /// <param name="status">The process status</param>
        /// <returns></returns>
        public Mock<ITarget> GetMockTarget(int status) => InitializeMockTarget(status);
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
                Activities = new List<Activity>
                {
                    new Activity()
                    {
                        Key = "UDW",
                        Source = "UDWFTP",
                        SourceDataOption = "UDWFTPOptions",
                        Target = "UDWDB",
                        TargetDataOption = "UDWDBOptions"
                    },
                    new Activity()
                    {
                        Key = "EStart",
                        Source = "EStartDB",
                        SourceDataOption = "EStartDBOptions",
                        Target = "EStartFTP",
                        TargetDataOption = "EStartFTPOptions"
                    },
                    new Activity()
                    {
                        Key = "NoSource",
                        Source = "",
                        SourceDataOption = "",
                        Target = "EStartFTP",
                        TargetDataOption = "EStartFTPOptions"
                    },
                    new Activity()
                    {
                        Key = "NoSourceOption",
                        Source = "EStartDB",
                        SourceDataOption = "NoOption",
                        Target = "EStartFTP",
                        TargetDataOption = "EStartFTPOptions"
                    },
                    new Activity()
                    {
                        Key = "NoTarget",
                        Source = "EStartDB",
                        SourceDataOption = "EStartDBOptions",
                        Target = "",
                        TargetDataOption = ""
                    },
                    new Activity()
                    {
                        Key = "NoTargetOption",
                        Source = "EStartDB",
                        SourceDataOption = "EStartDBOptions",
                        Target = "EStartFTP",
                        TargetDataOption = "NoOption"
                    }
                },
                DataOptions =  new List<DataOption>()
                {
                    new DataOption()
                    {
                        Key = "UDWDB",
                        Type = DataOptions.Mongo.GetDescription(),
                        Options = new Dictionary<string, string>()
                        {
                            { "ConnectionString" , "" },
                            { "DatabaseName" , "" }
                        }
                    },
                    new DataOption()
                    {
                        Key = "UDWFTP",
                        Type = DataOptions.FTP.GetDescription(),
                        Options = new Dictionary<string, string>()
                        {
                            { "FTPServer", "server" },
                            { "FTPInbox", "inbox" },
                            { "FTPOutbox", "outbox" }
                        }
                    },
                    new DataOption()
                    {
                        Key = "EStartDBOptions",
                        Type = DataOptions.Mongo.GetDescription(),
                        Options = new Dictionary<string, string>()
                        {
                            { "ConnectionString" , "" },
                            { "DatabaseName" , "" }
                        }
                    },
                    new DataOption()
                    {
                        Key = "EStartFTPOptions",
                        Type = DataOptions.FTP.GetDescription(),
                        Options = new Dictionary<string, string>()
                        {
                            { "FTPServer", "server" },
                            { "FTPInbox", "inbox" },
                            { "FTPOutbox", "outbox" }
                        }
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
        /// 
        /// </summary>
        /// <returns></returns>
        private MappingContext InitializeMappingContext()
        {
            return new MappingContext
            {
                Key = "UDW",
                Source = "UDWFTP",
                SourceType = DataOptions.FTP.GetDescription(),
                Target = "UDWDB",
                TargetType = DataOptions.Mongo.GetDescription(),
                SourceOptions = new Dictionary<string, string>(),
                TargetOptions = new Dictionary<string, string>()
            };
        }

        private List<MappingContext> GenerateMappingContexts()
        {
            return new List<MappingContext>()
            {
                new MappingContext
                {
                    Key = "UDW",
                    Source = "UDWFTP",
                    SourceType = DataOptions.FTP.GetDescription(),
                    Target = "UDWDB",
                    TargetType = DataOptions.Mongo.GetDescription(),
                    SourceOptions = new Dictionary<string, string>() {
                        { "FTPServer", "server" },
                        { "FTPInbox", "inbox" },
                        { "FTPOutbox", "outbox" } 
                    },
                    TargetOptions = new Dictionary<string, string>()
                    {
                        { "ConnectionString" , "" },
                        { "DatabaseName" , "" }
                    }
                },
                new MappingContext
                {
                    Key = "EStart",
                    Source = "EStartDB",
                    SourceType = DataOptions.FTP.GetDescription(),
                    Target = "EStartFTP",
                    TargetType = DataOptions.Mongo.GetDescription(),
                    SourceOptions = new Dictionary<string, string>()
                    {
                        { "ConnectionString" , "" },
                        { "DatabaseName" , "" }
                    },
                    TargetOptions = new Dictionary<string, string>() {
                        { "FTPServer", "server" },
                        { "FTPInbox", "inbox" },
                        { "FTPOutbox", "outbox" }
                    }
                },
                new MappingContext
                {
                    Key = "Test",
                    Source = "TestSource",
                    SourceType = DataOptions.FTP.GetDescription(),
                    Target = "TestTarget",
                    TargetType = DataOptions.Mongo.GetDescription(),
                    SourceOptions = new Dictionary<string, string>(),
                    TargetOptions = new Dictionary<string, string>()
                },
            };
        }
        /// <summary>
        /// A method to intialize a mock IMapperService
        /// </summary>
        /// <returns></returns>
        private Mock<IMapperService> InitializeMockMapper()
        {
            var mapper = new Mock<IMapperService>();
            mapper.SetReturnsDefault<MappingContext>(InitializeMappingContext());

            return mapper;
        }

        /// <summary>
        /// A method to initialize a mock IFTPService
        /// </summary>
        /// <returns></returns>
        private Mock<IFTPService> InitializeMockFTP()
        {
            var ftp = new Mock<IFTPService>();
            ftp.SetReturnsDefault<List<DataFeed>>(GetFeeds());
            return ftp;
        }

        /// <summary>
        /// A method to initialize a mock importer and customize the responses based on the input status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private Mock<ISource> InitializeMockSource(int status = (int)ProcessStatus.Success)
        {
            var source = new Mock<ISource>();
            source.SetReturnsDefault<string>("UDWFTP");
            source.SetReturnsDefault<Task<List<DataFeed>>>(Task.FromResult<List<DataFeed>>(InitializeDataFeed()));
            return source;
        }

        /// <summary>
        /// A method to initialize a mock exporter and customize the responses based on the input status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private Mock<ITarget> InitializeMockTarget(int status = (int)ProcessStatus.Success)
        {
            var target = new Mock<ITarget>();
            target.SetReturnsDefault<string>("UDWDB");
            target.SetReturnsDefault<Task<TargetResponse>>(Task.FromResult<TargetResponse>(new TargetResponse
            {
                Completed = new List<ImportData>()
                {
                    new ImportData()
                },
                Failed = new List<ImportData>()
                {

                },
                Partial = new List<ImportData>()
                {

                }
            }));
            return target;
        }

        /// <summary>
        /// A method to generate and initialize an enumerable of importer mock objects
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ISource> InitializeSources()
        {
            IList<ISource> importers = new List<ISource>();
            var udw = new Mock<ISource>();
            udw.SetReturnsDefault<string>("UDWFTP");

            importers.Add(udw.Object);

            
            var eStart = new Mock<ISource>();
            eStart.SetReturnsDefault<string>("EStartDB");

            importers.Add(eStart.Object);

            return importers;
        }

        /// <summary>
        /// A method to generate and initialize an enumerable of exporter mock objects
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ITarget> InitializeTargets()
        {
            IList<ITarget> exporters = new List<ITarget>();
            var udw = new Mock<ITarget>();
            udw.SetReturnsDefault<string>("UDWDB");

            exporters.Add(udw.Object);


            var eStart = new Mock<ITarget>();
            eStart.SetReturnsDefault<string>("EStartFTP");

            exporters.Add(eStart.Object);

            return exporters;
        }
        #endregion

    }
}
