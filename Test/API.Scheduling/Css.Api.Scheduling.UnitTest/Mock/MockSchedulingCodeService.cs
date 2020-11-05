using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockSchedulingCodeService
    {
        /// <summary>
        /// The SchedulingCodes
        /// </summary>
        public static List<SchedulingCode> schedulingCodes = new List<SchedulingCode>()
        {
            new SchedulingCode { Id=1,RefId=1,EmployeeId=1,IconId=1,CreatedBy="admin",CreatedDate=DateTime.UtcNow,Description="test",
             IsDeleted=false,ModifiedBy="",ModifiedDate=DateTime.UtcNow,PriorityNumber=1},
             new SchedulingCode { Id=2,RefId=2,EmployeeId=2,IconId=1,CreatedBy="admin",CreatedDate=DateTime.UtcNow,Description="test",
             IsDeleted=false,ModifiedBy="",ModifiedDate=DateTime.UtcNow,PriorityNumber=2},
             new SchedulingCode { Id=3,RefId=3,EmployeeId=3,IconId=1,CreatedBy="admin",CreatedDate=DateTime.UtcNow,Description="test",
             IsDeleted=false,ModifiedBy="",ModifiedDate=DateTime.UtcNow,PriorityNumber=3}
        };

        /// <summary>
        /// Gets the SchedulingCodes.
        /// </summary>
        /// <param name="SchedulingCodeParameters">The SchedulingCode parameters.</param>
        /// <returns></returns>
        public static CSSResponse GetSchedulingCodes(SchedulingCodeQueryParameters queryParameters)
        {
            return new CSSResponse(schedulingCodes, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the scheduling code ok result.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <returns></returns>
        public static CSSResponse GetSchedulingCodeOKResult(SchedulingCodeIdDetails schedulingCodeId)
        {
            var schedulingCode = schedulingCodes.Where(x => x.Id == schedulingCodeId.SchedulingCodeId && x.IsDeleted == false).FirstOrDefault();
            return new CSSResponse(schedulingCode, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the scheduling code not found result.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <returns></returns>
        public static CSSResponse GetSchedulingCodeNotFoundResult(SchedulingCodeIdDetails schedulingCodeId)
        {
            var schedulingCode = schedulingCodes.Where(x => x.Id == schedulingCodeId.SchedulingCodeId && x.IsDeleted == false).FirstOrDefault();
            return new CSSResponse(schedulingCode, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="createSchedulingCode">The create scheduling code.</param>
        /// <returns></returns>
        public static CSSResponse CreateSchedulingCode(CreateSchedulingCode createSchedulingCode)
        {
            SchedulingCode schedulingCode = new SchedulingCode()
            {
                Id=4,
                RefId= createSchedulingCode.RefId,
                CreatedBy= createSchedulingCode.CreatedBy,
                Description=createSchedulingCode.Description,
                IconId=createSchedulingCode.IconId,
                PriorityNumber=createSchedulingCode.PriorityNumber
            };

            schedulingCodes.Add(schedulingCode);

            return new CSSResponse(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCode.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Deletes the scheduling code ok result.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <returns></returns>
        public static CSSResponse DeleteSchedulingCodeOKResult(SchedulingCodeIdDetails schedulingCodeId)
        {
            var schedulingCode = schedulingCodes.Where(x => x.Id == schedulingCodeId.SchedulingCodeId && x.IsDeleted == false).FirstOrDefault();
            schedulingCodes.Remove(schedulingCode);
            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the scheduling code not found result.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <returns></returns>
        public static CSSResponse DeleteSchedulingCodeNotFoundResult(SchedulingCodeIdDetails schedulingCodeId)
        {
            return new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Updates the scheduling code ok result.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <param name="updateSchedulingCode">The update scheduling code.</param>
        /// <returns></returns>
        public static object UpdateSchedulingCodeOKResult(SchedulingCodeIdDetails schedulingCodeIdDetails, UpdateSchedulingCode updateSchedulingCode)
        {
            var schedulingCode = schedulingCodes.Where(x => x.Id == schedulingCodeIdDetails.SchedulingCodeId && x.IsDeleted == false).FirstOrDefault();
            schedulingCode.PriorityNumber = updateSchedulingCode.PriorityNumber;
            schedulingCode.Description = updateSchedulingCode.Description;
            schedulingCode.ModifiedBy = updateSchedulingCode.ModifiedBy;
            schedulingCode.IconId = updateSchedulingCode.IconId;
            schedulingCode.ModifiedDate = DateTime.UtcNow;

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the scheduling code not found result.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <param name="updateSchedulingCode">The update scheduling code.</param>
        /// <returns></returns>
        public static object UpdateSchedulingCodeNotFoundResult(SchedulingCodeIdDetails schedulingCodeIdDetails, UpdateSchedulingCode updateSchedulingCode)
        {
            return new CSSResponse(HttpStatusCode.NotFound);
        }
    }
}
