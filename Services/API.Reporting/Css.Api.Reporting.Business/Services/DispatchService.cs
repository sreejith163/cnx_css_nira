using Css.Api.Core.Models.Enums;
using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
    /// <summary>
    /// The dispatch service
    /// </summary>
    public class DispatchService : IDispatchService
    {
        #region Private Properties

        /// <summary>
        /// The FTP service
        /// </summary>
        private readonly IFTPService _ftp;

        /// <summary>
        /// The agent repository
        /// </summary>
        private readonly IAgentRepository _agentRepository;

        /// <summary>
        /// The agent scheduling group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize the properties
        /// </summary>
        /// <param name="ftp"></param>
        /// <param name="agentRepository"></param>
        /// <param name="agentSchedulingGroupRepository"></param>
        public DispatchService(IFTPService ftp, IAgentRepository agentRepository, IAgentSchedulingGroupRepository agentSchedulingGroupRepository)
        {
            _ftp = ftp;
            _agentRepository = agentRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// The method to push agent info to the respective target
        /// </summary>
        /// <param name="target"></param>
        /// <returns>An instance of DispatchResponse</returns>
        public async Task<DispatchResponse> PushAgentInfo(ActivityOrigin target)
        {
            DispatchResponse response = new DispatchResponse();
            if(target == ActivityOrigin.EStart)
            {
                response = await PushToEStart();
            }
            else
            {
                response.Status = ProcessStatus.Failed.ToString();
                response.Message = Messages.AgentExportNotAllowed;
            }
            return response;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// The helper method to push data to Estart FTP server
        /// </summary>
        /// <returns></returns>
        private async Task<DispatchResponse> PushToEStart()
        {
            List<DispatchData> data = new List<DispatchData>();
            var agentSchedulingGroups = await _agentSchedulingGroupRepository.GetAgentSchedulingGroups(true);
            agentSchedulingGroups.ForEach(async asg =>
            {
                var agents = await _agentRepository.GetAgents(asg.AgentSchedulingGroupId);
                var agentString = string.Join("\n", agents.Select(x => x.Ssn).ToList());
                if (!string.IsNullOrWhiteSpace(agentString))
                {
                    var filename = _ftp.GetEmployeesFileName(asg.RefId?.ToString() ?? asg.Name.Substring(0, 10).Trim());
                    var status = _ftp.Write(filename, agentString);
                    if(status)
                    {
                        data.Add(new DispatchData()
                        {
                            DataSet = filename,
                            Bytes = Encoding.Default.GetBytes(agentString).Length
                        });
                    }
                }
            });
            
            if(data.Any())
            {
                return new DispatchResponse()
                {
                    Data = data,
                    Status = ProcessStatus.Success.ToString()
                };
            }
            else
            {
                return new DispatchResponse()
                {
                    Status = ProcessStatus.Failed.ToString(),
                    Message = Messages.NoAgentsEStartProvisioned
                };
            }
        }
        #endregion
    }
}
