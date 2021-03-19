using Css.Api.Job.Business.Interfaces;
using Css.Api.Job.Models.DTO.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Css.Api.Job.Controllers
{
    /// <summary>
    /// The jobs controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        /// <summary>
        /// The jobs
        /// </summary>
        private readonly IOptions<Jobs> _jobs;

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="jobs"></param>
        public JobsController(IOptions<Jobs> jobs)
        {
            _jobs = jobs;
        }

        /// <summary>
        /// The HTTP GET endpoint
        /// </summary>
        /// <returns>All the existing job configured in the system</returns>
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(_jobs.Value);
        }

    }
}
