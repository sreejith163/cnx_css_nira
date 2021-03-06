﻿using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Models.Domain;
using System.Collections.Generic;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockSchedulingCodeIconData
    {
        /// <summary>
        /// The scheduling code icons
        /// </summary>
        private readonly List<SchedulingCodeIcon> schedulingCodeIcons = new List<SchedulingCodeIcon>()
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
        public CSSResponse GetSchedulingCodeIcons()
        {
            return new CSSResponse(schedulingCodeIcons, HttpStatusCode.OK);
        }
    }
}
