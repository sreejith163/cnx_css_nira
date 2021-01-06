using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.Domain;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The service to process the UDW Imports
    /// </summary>
    public class UDWImport : IImporter
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
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the Importer which is used to map in the mapper json
        /// </summary>
        public string Name => "UDWImporter";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="agentRepository"></param>
        public UDWImport(IMapper mapper, IAgentRepository agentRepository, IUnitOfWork uow)
        {
            _mapper = mapper;
            _agentRepository = agentRepository;
            _uow = uow;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The business logic to process the import is written here
        /// </summary>
        /// <param name="data">The input byte[] to be processed</param>
        /// <returns>An instance of ImportResponse</returns>
        public async Task<ImportResponse> Import(byte[] data)
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
                    return new ImportResponse()
                    {
                        Status = (int)ProcessStatus.Partial,
                        Metadata = metadata
                    };
                }

                return new ImportResponse()
                {
                    Status = (int)ProcessStatus.Success
                };
            }
            catch(Exception ex)
            {
                return new ImportResponse()
                {
                    Status = (int)ProcessStatus.Failed,
                    Metadata = ex.Message + "at - \n\n" + ex.StackTrace
                };
            }
        }
        #endregion

        #region Private Methods
        
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
            if(newAgents.Any())
            {
                mismatch.NewAgents = newAgents;
            }

            if(changeAgents.Any())
            {
                mismatch.ChangedAgents = changeAgents;
            }

            if( (mismatch.NewAgents != null && mismatch.NewAgents.Any()) 
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
