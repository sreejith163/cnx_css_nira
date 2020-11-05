using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockSchedulingCodeData
    {
        /// <summary>
        /// The SchedulingCodes
        /// </summary>
        private List<SchedulingCode> schedulingCodesDB = new List<SchedulingCode>()
        {
            new SchedulingCode { Id = 1, RefId = 1, Description = "test1", PriorityNumber = 1, EmployeeId = 1, IconId = 1, CreatedBy = "admin",
                                 CreatedDate = DateTime.UtcNow },
            new SchedulingCode { Id = 2, RefId = 1, Description = "test2", PriorityNumber = 2, EmployeeId = 1, IconId = 2, CreatedBy = "admin",
                                 CreatedDate = DateTime.UtcNow },
            new SchedulingCode { Id = 3, RefId = 1, Description = "test3", PriorityNumber = 3, EmployeeId = 1, IconId = 3, CreatedBy = "admin",
                                 CreatedDate = DateTime.UtcNow }
        };

        /// <summary>
        /// Gets the SchedulingCodes.
        /// </summary>
        /// <param name="SchedulingCodeParameters">The SchedulingCode parameters.</param>
        /// <returns></returns>
        public CSSResponse GetSchedulingCodes(SchedulingCodeQueryParameters queryParameters)
        {
            var schedulingCodes = schedulingCodesDB.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize);
            return new CSSResponse(schedulingCodes, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <returns></returns>
        public CSSResponse GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeId)
        {
            var schedulingCode = schedulingCodesDB.Where(x => x.Id == schedulingCodeId.SchedulingCodeId && x.IsDeleted == false).FirstOrDefault();
            return schedulingCode != null ? new CSSResponse(schedulingCode, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="createSchedulingCode">The create scheduling code.</param>
        /// <returns></returns>
        public CSSResponse CreateSchedulingCode(CreateSchedulingCode createSchedulingCode)
        {
            SchedulingCode schedulingCode = new SchedulingCode()
            {
                Id = 4,
                RefId = createSchedulingCode.RefId,
                CreatedBy = createSchedulingCode.CreatedBy,
                Description = createSchedulingCode.Description,
                IconId = createSchedulingCode.IconId,
                PriorityNumber = createSchedulingCode.PriorityNumber
            };

            schedulingCodesDB.Add(schedulingCode);

            return new CSSResponse(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCode.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <param name="updateSchedulingCode">The update scheduling code.</param>
        /// <returns></returns>
        public CSSResponse UpdateSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails, UpdateSchedulingCode updateSchedulingCode)
        {
            if (!schedulingCodesDB.Exists(x => x.IsDeleted == false && x.Id == schedulingCodeIdDetails.SchedulingCodeId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var schedulingCode = schedulingCodesDB.Where(x => x.IsDeleted == false && x.Id == schedulingCodeIdDetails.SchedulingCodeId).FirstOrDefault();
            schedulingCode.PriorityNumber = updateSchedulingCode.PriorityNumber;
            schedulingCode.Description = updateSchedulingCode.Description;
            schedulingCode.ModifiedBy = updateSchedulingCode.ModifiedBy;
            schedulingCode.IconId = updateSchedulingCode.IconId;
            schedulingCode.ModifiedDate = DateTime.UtcNow;

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteSchedulingCodes(SchedulingCodeIdDetails schedulingCodeIdDetails)
        {
            if (!schedulingCodesDB.Exists(x => x.IsDeleted == false && x.Id == schedulingCodeIdDetails.SchedulingCodeId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var schedulingCode = schedulingCodesDB.Where(x => x.IsDeleted == false && x.Id == schedulingCodeIdDetails.SchedulingCodeId).FirstOrDefault();
            schedulingCodesDB.Remove(schedulingCode);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
