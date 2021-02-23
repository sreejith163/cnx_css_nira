using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.UserLanguage;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Admin.Controllers

{    /// <summary>Controller for handling Agent resource</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserLanguageController : ControllerBase
    {
        /// <summary>
        /// The language service
        /// </summary>
        private readonly IUserLanguageService _userLanguageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLanguageController"/> class.
        /// </summary>
        /// <param name="userLanguageService">The user language service.</param>
        public UserLanguageController(IUserLanguageService userLanguageService)
        {
            _userLanguageService = userLanguageService;
        }

        /// <summary>
        /// Gets the User Language Preference.
        /// </summary>
        /// <param name="employeeId">The employeeId identifier.</param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetUserLanguagePreference(string employeeId)
        {
            var result = await _userLanguageService.GetUserLanguagePreference(employeeId);

            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates or Creates the User Language Preference.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="userLanguagePreference">The userLanguagePreference identifier.</param>
        /// <returns></returns>
        [HttpPost("{employeeId}")]
        public async Task<IActionResult> UpdateUserLanguagePreference(string employeeId, [FromBody] UpdateUserLanguagePreferenceDTO userLanguagePreference)
        {
            var result = await _userLanguageService.UpdateUserLanguagePreference(new UserLanguageEmployeeIdDetails { EmployeeId = employeeId}, userLanguagePreference);

            return StatusCode((int)result.Code, result.Value);
        }


    }
}
