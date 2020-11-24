using Css.Api.SetupMenu.Business.Interfaces;
using Css.Api.SetupMenu.Models.DTO.Request.ClientLOBGroup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.SetupMenu.Controllers
{
    /// <summary>Controller for handling Client LOB group resource</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientLOBGroupsController : ControllerBase
    {

        /// <summary>The client lob group service</summary>
        private readonly IClientLOBGroupService _clientLOBGroupService;

        /// <summary>Initializes a new instance of the <see cref="ClientLOBGroupsController" /> class.</summary>
        /// <param name="clientLOBGroupService">The client lob group service.</param>
        public ClientLOBGroupsController(IClientLOBGroupService clientLOBGroupService)
        {
            _clientLOBGroupService = clientLOBGroupService;
        }

        /// <summary>Gets the client lob groups.</summary>
        /// <param name="clientLOBQueryParameters">The client lob query parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetClientLOBGroups([FromQuery] ClientLOBGroupQueryParameter clientLOBQueryParameters)
        {
            var result = await _clientLOBGroupService.GetClientLOBGroups(clientLOBQueryParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Gets the client lob group.</summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("{clientLOBGroupId}")]
        public async Task<IActionResult> GetClientLOBGroup(int clientLOBGroupId)
        {
            var result = await _clientLOBGroupService.GetClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Creates the client lob group.</summary>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateClientLOBGroup([FromBody] CreateClientLOBGroup clientLOBGroupDetails)
        {
            var result = await _clientLOBGroupService.CreateClientLOBGroup(clientLOBGroupDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Updates the client lob group.</summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPut("{clientLOBGroupId}")]
        public async Task<IActionResult> UpdateClientLOBGroup(int clientLOBGroupId, [FromBody] UpdateClientLOBGroup clientLOBGroupDetails)
        {
            var result = await _clientLOBGroupService.UpdateClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }, clientLOBGroupDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Deletes the client lob group.</summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpDelete("{clientLOBGroupId}")]
        public async Task<IActionResult> DeleteClientLOBGroup(int clientLOBGroupId)
        {
            var result = await _clientLOBGroupService.DeleteClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}

