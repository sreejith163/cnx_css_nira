﻿using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.Client;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Setup.Controllers
{
    /// <summary>
    /// Controller for handling the client resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        /// <summary>
        /// The client service
        /// </summary>
        private readonly IClientService _clientService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController"/> class.
        /// </summary>
        /// <param name="clientService">The client service.</param>
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] ClientQueryParameters clientParameters)
        {
            var result = await _clientService.GetClients(clientParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClient(int clientId)
        {
            var result = await _clientService.GetClient(new ClientIdDetails { ClientId = clientId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClient clientDetails)
        {
            var result = await _clientService.CreateClient(clientDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        [HttpPut("{clientId}")]
        public async Task<IActionResult> UpdateClient(int clientId, [FromBody] UpdateClient clientDetails)
        {
            var result = await _clientService.UpdateClient(new ClientIdDetails { ClientId = clientId }, clientDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            var result = await _clientService.DeleteClient(new ClientIdDetails { ClientId = clientId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
