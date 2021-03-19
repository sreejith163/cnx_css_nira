using Css.Api.Job.Business.Interfaces;
using Css.Api.Job.Models.DTO.Configurations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Job.Business.Services
{
    /// <summary>
    /// The HTTP helper service
    /// </summary>
    public class HttpService : IHttpService
    {
        #region Private Properties

        /// <summary>
        /// The service provider
        /// </summary>
        private readonly IHttpClientFactory _httpClient;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize properties
        /// </summary>
        /// <param name="httpClient"></param>
        public HttpService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Method to create the HttpRequestMessage using the cron job attributes
        /// </summary>
        /// <param name="job">The instance of CronJob</param>
        /// <returns>An instance of HttpRequestMessage</returns>
        public HttpRequestMessage CreateHttpRequestMessage(CronJob job)
        {
            var reqMessage = new HttpRequestMessage();
            reqMessage.RequestUri = new Uri(job.Url);

            if (job.Headers != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in job.Headers)
                {
                    reqMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            reqMessage.Method = new HttpMethod(job.Method);

            if (!string.IsNullOrWhiteSpace(job.Content))
            {
                reqMessage.Content = new StringContent(job.Content);
            }

            return reqMessage;
        }

        /// <summary>
        /// Method to create the HttpRequestMessage using the cron job attributes and the input content
        /// </summary>
        /// <param name="job">The instance of CronJob</param>
        /// <param name="Content">The request body contents</param>
        /// <returns>An instance of HttpRequestMessage</returns>
        public HttpRequestMessage CreateHttpRequestMessage(CronJob job, string Content)
        {
            var reqMessage = new HttpRequestMessage();
            reqMessage.RequestUri = new Uri(job.Url);

            if (job.Headers != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in job.Headers)
                {
                    reqMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            reqMessage.Method = new HttpMethod(job.Method);

            if (!string.IsNullOrWhiteSpace(Content))
            {
                reqMessage.Content = new StringContent(Content, Encoding.UTF8, "application/json"); 
            }

            return reqMessage;
        }

        /// <summary>
        /// Method to send the http request message
        /// </summary>
        /// <param name="reqMessage">The request message</param>
        /// <returns>The instance of HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage reqMessage)
        {
            var client = _httpClient.CreateClient();
            var responseMessage = await client.SendAsync(reqMessage);
            client.Dispose();
            return responseMessage;
        }

        /// <summary>
        /// Method to send multiple http request messages
        /// </summary>
        /// <param name="reqMessages">List of request messages</param>
        /// <returns>The list of instances of HttpResponseMessage</returns>
        public async Task<List<HttpResponseMessage>> SendAsync(List<HttpRequestMessage> reqMessages)
        {
            List<HttpResponseMessage> responseMessages = new List<HttpResponseMessage>();
            var client = _httpClient.CreateClient();
            foreach(var request in reqMessages)
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch(Exception ex)
                {
                    response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
                    response.Content = new StringContent(ex.Message);
                }
                responseMessages.Add(response);
            }
            client.Dispose();
            return responseMessages;
        }
        #endregion
    }
}
