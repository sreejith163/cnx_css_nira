using Css.Api.Core.Models.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.ForecastScreen
{
    public class ForeCastScreenQueryParameter : QueryStringParameters
    {
        public ForeCastScreenQueryParameter()
        {
            OrderBy = "skillGroupId";
        }
    }
}
