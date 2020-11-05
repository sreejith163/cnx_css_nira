using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockSchedulingCodeIconService
    {
        /// <summary>
        /// The scheduling code icons
        /// </summary>
        public static List<SchedulingCodeIcon> schedulingCodeIcons = new List<SchedulingCodeIcon>()
        {
            new SchedulingCodeIcon { Id = 1, Value = "1F30D", Description = "Earth Globe Europe-Africa" },
            new SchedulingCodeIcon { Id = 1, Value = "1F347", Description = "Grapes" },
            new SchedulingCodeIcon { Id = 1, Value = "1F383", Description = "Jack-O-Lantern" },
            new SchedulingCodeIcon { Id = 1, Value = "1F3C1", Description = "Chequered Flag" },
            new SchedulingCodeIcon { Id = 1, Value = "1F3E7", Description = "Automated Teller Machine" }
        };

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        /// <returns></returns>
        public static CSSResponse GetSchedulingCodeIcons()
        {
            return new CSSResponse(schedulingCodeIcons, HttpStatusCode.OK);
        }
    }
}
