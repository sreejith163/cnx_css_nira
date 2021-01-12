using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Models.Domain;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Targets
{
    public class UDWImportTarget : ITarget
    {
        #region Private Properties

        /// <summary>
        /// The automapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The agent repository
        /// </summary>
        private readonly IAgentRepository _agentRepository;

        /// <summary>
        /// The transation support of mongo
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// The FTP service
        /// </summary>
        private readonly IFTPService _ftp;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the Importer which is used to map in the mapper json
        /// </summary>
        public string Name => "UDWImpTar";
        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="agentRepository"></param>
        public UDWImportTarget(IMapper mapper, IAgentRepository agentRepository, IUnitOfWork uow, IFTPService ftp)
        {
            _mapper = mapper;
            _agentRepository = agentRepository;
            _uow = uow;
            _ftp = ftp;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to push data to the destination
        /// </summary>
        /// <param name="feeds">List of instances of DataFeed (sources)</param>
        /// <returns>An instance of ImportResponse</returns>
        public async Task<TargetResponse> Push(List<DataFeed> feeds)
        {
            TargetResponse response = new TargetResponse();   
            foreach (DataFeed feed in feeds)
            {
                DataResponse resp = await Import(feed.Content);
                feed.Metadata = resp.Metadata;

                List<int> processed = new List<int>() { (int)ProcessStatus.Success, (int)ProcessStatus.Partial };
                if (processed.Contains(resp.Status))
                {
                    await _ftp.MoveToProcessedFolder(feed);
                }
                else
                {
                    await _ftp.MoveToFailedFolder(feed);
                }

                ImportData strategyFeed = new ImportData();
                strategyFeed.Bytes = feed.Content.Length;
                strategyFeed.Source = feed.Path;

                switch (resp.Status)
                {
                    case (int)ProcessStatus.Success:
                        response.Completed.Add(strategyFeed);
                        break;
                    case (int)ProcessStatus.Failed:
                        response.Failed.Add(strategyFeed);
                        break;
                    case (int)ProcessStatus.Partial:
                        response.Partial.Add(strategyFeed);
                        break;
                    default:
                        break;
                }
            }
            return response;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// The business logic to process the import is written here
        /// </summary>
        /// <param name="data">The input byte[] to be processed</param>
        /// <returns>An instance of ImportResponse</returns>
        private async Task<DataResponse> Import(byte[] data)
        {
            try
            {
                XMLParser<UDWAgentList> parser = new XMLParser<UDWAgentList>();
                UDWAgentList root = parser.Deserialize(data);
                var agents = _mapper.Map<List<Agent>>(root.NewAgents)
                    .Union(_mapper.Map<List<Agent>>(root.ChangedAgents))
                    .ToList();

                _agentRepository.UpsertAsync(agents);
                await _uow.Commit();

                var metadata = CheckImport(root, agents);

                if (!string.IsNullOrWhiteSpace(metadata))
                {
                    return new DataResponse()
                    {
                        Status = (int)ProcessStatus.Partial,
                        Metadata = metadata
                    };
                }

                return new DataResponse()
                {
                    Status = (int)ProcessStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Status = (int)ProcessStatus.Failed,
                    Metadata = ex.Message + "at - \n\n" + ex.StackTrace
                };
            }
        }

        /// <summary>
        /// The method to check any mismatches in the source and destination data
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns>Returns an empty string if it matches, else the serialized array of all the partially imported sources data.</returns>
        private string CheckImport(UDWAgentList source, List<Agent> dest)
        {
            var metadata = string.Empty;

            var newAgents = (from ag in source.NewAgents
                             join up in dest on ag.SSN equals up.Ssn
                             where (ag.SenDate != null && up.SenDate == null)
                             || (ag.SenExt != null && up.SenExt == null)
                             || (ag.AgentData.Count != up.AgentData.Count)
                             select ag).ToList();

            var changeAgents = (from ag in source.ChangedAgents
                                join up in dest on ag.SSN equals up.Ssn
                                where (ag.SenDate != null && up.SenDate == null)
                                || (ag.SenExt != null && up.SenExt == null)
                                || (ag.AgentData.Count != up.AgentData.Count)
                                select ag).ToList();

            var mismatch = new UDWAgentList();
            if (newAgents.Any())
            {
                mismatch.NewAgents = newAgents;
            }

            if (changeAgents.Any())
            {
                mismatch.ChangedAgents = changeAgents;
            }

            if ((mismatch.NewAgents != null && mismatch.NewAgents.Any())
                || (mismatch.ChangedAgents != null && mismatch.ChangedAgents.Any())
            )
            {
                XMLParser<UDWAgentList> parser = new XMLParser<UDWAgentList>();
                metadata = parser.Serialize(mismatch);
            }

            return metadata;
        }
        #endregion
    }
}
