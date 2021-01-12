using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Css.Api.Reporting.Controllers
{
    /// <summary>
    /// Controller for handling the all imports
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        /// <summary>
        /// The strategy for import
        /// </summary>
        private readonly IImportStrategy _importStrategy;

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="importStrategy"></param>
        public ImportController(IImportStrategy importStrategy)
        {
            _importStrategy = importStrategy;
        }

        /// <summary>
        /// The import method using the HTTP POST call
        /// </summary>
        [HttpPost]
        public async Task<TargetResponse> Post()
        {
            return await _importStrategy.Process();
        }

    }
}
