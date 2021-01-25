using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>controller for entity hierarchy</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EntityHierarchyController : ControllerBase
    {
        /// <summary>The entity hierarchy service</summary>
        private readonly IEntityHierarchyService _entityHierarchyService;

        /// <summary>Initializes a new instance of the <see cref="EntityHierarchyController" /> class.</summary>
        /// <param name="entityHierarchyService">The entity hierarchy service.</param>
        public EntityHierarchyController(IEntityHierarchyService entityHierarchyService)
        {
            _entityHierarchyService = entityHierarchyService;
        }

        /// <summary>Gets the entity hierarchy.</summary>
        /// <param name="clientId">The client identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetEntityHierarchy(int clientId)
        {
            var result = await _entityHierarchyService.GetEntityHierarchy(new ClientIdDetails { ClientId = clientId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}