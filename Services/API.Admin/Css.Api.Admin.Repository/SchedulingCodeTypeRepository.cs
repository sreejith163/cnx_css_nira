﻿using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using Css.Api.Core.Models.DTO.Response;

namespace Css.Api.Admin.Repository
{
    public class SchedulingCodeTypeRepository : GenericRepository<SchedulingCodeType>, ISchedulingCodeTypeRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeTypeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="">The .</param>
        public SchedulingCodeTypeRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValue>> GetSchedulingCodeTypes()
        {
            var schedulingCodeTypes = FindAll().ProjectTo<KeyValue>(_mapper.ConfigurationProvider).ToList();
            return await Task.FromResult(schedulingCodeTypes);
        }
    }
}
