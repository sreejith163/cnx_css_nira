using Css.Api.Admin.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface IAuthRepository
    {
        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="credentialsViewModel">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<NonSsoModel> Login(CredentialsViewModel credentialsViewModel);

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="credentialsViewModel">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<NonSsoModel> GetUserDetails(CredentialsViewModel credentialsViewModel);
    }
}
