using Css.Api.Core.Models.DTO.Responses;
using Css.Api.Scheduling.Models.DTO.Requests.Client;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IClientService
    {
        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetClients(ClientQueryParameters clientParameters);

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetClient(ClientIdDetails clientIdDetails);

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateClient(CreateClient clientDetails);

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateClient(ClientIdDetails clientIdDetails, UpdateClient clientDetails);

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteClient(ClientIdDetails clientIdDetails);
    }
}
