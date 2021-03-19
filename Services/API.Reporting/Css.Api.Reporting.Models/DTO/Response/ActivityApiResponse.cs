using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Response
{
    public class ActivityApiResponse : StrategyResponse
    {
        public object Data { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        public ActivityApiResponse()
        {
            StatusCode = HttpStatusCode.OK;
        }
    }
}
