using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iAuthService;


        /// <summary>
        /// Initializes a new instance of the <see cref="LogController"/> class.
        /// </summary>
        /// <param name="authService">The role service.</param>
        public AuthController(IAuthService authService)
        {
            _iAuthService = authService;
        }

        /// <summary>
        /// login
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpGet("{username}/key/{password}")]
        public async Task<IActionResult> Login(string username, string password)
        {
         
            var result = await _iAuthService.Login(new CredentialsViewModel { UserName = username, Password = password });
            return StatusCode((int)result.Code, result.Value);
        }
      
    }
}
