using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    public class SchedulingCodeIconService : ISchedulingCodeIconService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public SchedulingCodeIconService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        /// <returns></returns>
        public async Task<CSSResponse> GetSchedulingCodeIcons()
        {
            var schedulingCodes = await _repository.SchedulingCodeIcons.GetSchedulingCodeIcons();
            return new CSSResponse(schedulingCodes, HttpStatusCode.OK);
        }
    }
}
