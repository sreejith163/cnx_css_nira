﻿using Css.Api.Core.Models.Domain;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using AutoMapper;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Core.DataAccess.Repository.SQL;

namespace Css.Api.Admin.Repository
{
    public class SchedulingCodeRepository : GenericRepository<SchedulingCode>, ISchedulingCodeRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public SchedulingCodeRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeParameters">The scheduling code parameters.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetSchedulingCodes(SchedulingCodeQueryParameters schedulingCodeParameters)
        {
            var schedulingCodes = FindByCondition(x => x.IsDeleted == false);

            var filteredSchedulingCodes = FilterSchedulingCodes(schedulingCodes, schedulingCodeParameters);

            var sortedSchedulingCodes = SortHelper.ApplySort(filteredSchedulingCodes, schedulingCodeParameters.OrderBy);

            var pagedSchedulingCodes = sortedSchedulingCodes;

            if (!schedulingCodeParameters.SkipPageSize)
            {
                pagedSchedulingCodes = pagedSchedulingCodes
                    .Skip((schedulingCodeParameters.PageNumber - 1) * schedulingCodeParameters.PageSize)
                    .Take(schedulingCodeParameters.PageSize);
            }

            pagedSchedulingCodes = pagedSchedulingCodes
                .Include(x => x.Icon)
                .Include(x => x.SchedulingTypeCode)
                .ThenInclude(x => x.SchedulingCodeType);

            var mappedSchedulingCodes = pagedSchedulingCodes
                .ProjectTo<SchedulingCodeDTO>(_mapper.ConfigurationProvider);

            var shapedSchedulingCodes = DataShaper.ShapeData(mappedSchedulingCodes, schedulingCodeParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSchedulingCodes, filteredSchedulingCodes.Count(), schedulingCodeParameters.PageNumber, schedulingCodeParameters.PageSize);
        }

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        public async Task<SchedulingCode> GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails)
        {
            var schedulingCode = FindByCondition(x => x.Id == schedulingCodeIdDetails.SchedulingCodeId && x.IsDeleted == false)
                .Include(x => x.Icon)
                .Include(x => x.SchedulingTypeCode)
                .ThenInclude(x => x.SchedulingCodeType)
                .SingleOrDefault();

            return await Task.FromResult(schedulingCode);
        }

        /// <summary>
        /// Gets the scheduling codes by description.
        /// </summary>
        /// <param name="schedulingCodeNameDetails">The scheduling code name details.</param>
        /// <param name="schedulingIconIdDetails">The scheduling icon identifier details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetSchedulingCodesByDescriptionAndIcon(SchedulingCodeNameDetails schedulingCodeNameDetails, SchedulingIconIdDetails schedulingIconIdDetails)
        {
            var schedulingCodes = FindByCondition(x => x.IsDeleted == false && (x.IconId == schedulingIconIdDetails.SchedulingIconId ||
                                                       string.Equals(x.Description.Trim(), schedulingCodeNameDetails.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(schedulingCodes);
        }

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        public void CreateSchedulingCode(SchedulingCode schedulingCode)
        {
            Create(schedulingCode);
        }

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        public void UpdateSchedulingCode(SchedulingCode schedulingCode)
        {
            Update(schedulingCode);
        }

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        public void DeleteSchedulingCode(SchedulingCode schedulingCode)
        {
            Delete(schedulingCode);
        }

        /// <summary>
        /// Searches the name of the by.
        /// </summary>
        /// <param name="schedulingCodes">The scheduling codes.</param>
        /// <param name="schedulingCodeParameters">The scheduling code parameters.</param>
        /// <returns></returns>
        private IQueryable<SchedulingCode> FilterSchedulingCodes(IQueryable<SchedulingCode> schedulingCodes, SchedulingCodeQueryParameters schedulingCodeParameters)
        {
            if (!schedulingCodes.Any())
            {
                return schedulingCodes;
            }

            if (schedulingCodeParameters.ActivityCodes.Any())
            {
                schedulingCodes = schedulingCodes.Where(x => schedulingCodeParameters.ActivityCodes.Contains(x.Description));
            }

            if (!string.IsNullOrWhiteSpace(schedulingCodeParameters.SearchKeyword))
            {
                schedulingCodes = schedulingCodes.Where(o => o.Description.ToLower().Contains(schedulingCodeParameters.SearchKeyword.Trim().ToLower()) ||
                                                  o.CreatedBy.ToLower().Contains(schedulingCodeParameters.SearchKeyword.Trim().ToLower()) ||
                                                  o.ModifiedBy.ToLower().Contains(schedulingCodeParameters.SearchKeyword.Trim().ToLower()));
            }

            return schedulingCodes;
        }
    }
}
