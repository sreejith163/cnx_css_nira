using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Repository.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
    public class SchedulingCodeTypeService : ISchedulingCodeTypeService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeTypeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public SchedulingCodeTypeService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        /// <returns></returns>
        public async Task<CSSResponse> GetSchedulingCodeTypes()
        {
            var schedulingCodes = await _repository.SchedulingCodeTypes.GetSchedulingCodeTypes();
            return new CSSResponse(schedulingCodes, HttpStatusCode.OK);
        }
    }
}
