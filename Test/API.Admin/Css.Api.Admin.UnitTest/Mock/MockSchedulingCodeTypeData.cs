using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Models.Domain;
using System.Collections.Generic;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockSchedulingCodeTypeData
    {
        /// <summary>
        /// The scheduling code types
        /// </summary>
        private readonly List<SchedulingCodeType> schedulingCodeTypes = new List<SchedulingCodeType>()
        {
            new SchedulingCodeType {Id=1,Description="test1",Value="A"},
            new SchedulingCodeType {Id=2,Description="test2",Value="B"},
            new SchedulingCodeType { Id = 3, Description = "test3", Value = "C" },
            new SchedulingCodeType { Id = 4, Description = "test4", Value = "D" }
        };

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        /// <returns></returns>
        public CSSResponse GetSchedulingCodeTypes()
        {
            return new CSSResponse(schedulingCodeTypes, HttpStatusCode.OK);
        }
    }
}
