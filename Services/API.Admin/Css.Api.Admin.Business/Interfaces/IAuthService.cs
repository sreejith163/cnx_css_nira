using Css.Api.Admin.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="credentials">The role parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> Login(CredentialsViewModel credentials);
    }
}
