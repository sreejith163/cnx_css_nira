using Css.Api.Job.Models.DTO.Configurations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Job.Business.Interfaces
{
    /// <summary>
    /// An interface for the http helper service
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Method to create the HttpRequestMessage using the cron job attributes
        /// </summary>
        /// <param name="job"></param>
        /// <returns>An instance of HttpRequestMessage</returns>
        HttpRequestMessage CreateHttpRequestMessage(CronJob job);

        /// <summary>
        /// Method to create the HttpRequestMessage using the cron job attributes and the input content
        /// </summary>
        /// <param name="job">The instance of CronJob</param>
        /// <param name="Content">The request body contents</param>
        /// <returns>An instance of HttpRequestMessage</returns>
        HttpRequestMessage CreateHttpRequestMessage(CronJob job, string Content);

        /// <summary>
        /// Method to send the http request message
        /// </summary>
        /// <param name="reqMessage"></param>
        /// <returns>The instance of HttpResponseMessage</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage reqMessage);

        /// <summary>
        /// Method to send multiple http request messages
        /// </summary>
        /// <param name="reqMessages">List of request messages</param>
        /// <returns>The list of instances of HttpResponseMessage</returns>
        Task<List<HttpResponseMessage>> SendAsync(List<HttpRequestMessage> reqMessages);
    }
}
