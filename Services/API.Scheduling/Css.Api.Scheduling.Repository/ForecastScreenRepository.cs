using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ForecastScreen;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Response.ForecastScreen;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class ForecastScreenRepository : GenericRepository<ForecastScreen>, IForecastScreenRepository
    {
        private readonly IMapper _mapper;
        public ForecastScreenRepository(IMongoContext mongoContext,
          IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }
        public async Task<PagedList<Entity>> GetForecastScreens(ForeCastScreenQueryParameter forecastScreenQueryparameter)
        {
            var forecastScreens = FilterBy(x => true);

            var filteredForecastScreens = FilterForecastscreen(forecastScreens, forecastScreenQueryparameter);

            var sortedForecastScreens = SortHelper.ApplySort(filteredForecastScreens, forecastScreenQueryparameter.OrderBy);

            var pagedForecastScreens = sortedForecastScreens
                .Skip((forecastScreenQueryparameter.PageNumber - 1) * forecastScreenQueryparameter.PageSize)
                .Take(forecastScreenQueryparameter.PageSize);

            var mappedForecastScreens = pagedForecastScreens.ProjectTo<ForecastScreenDTO>(_mapper.ConfigurationProvider);

            var shapedForecastScreens = DataShaper.ShapeData(mappedForecastScreens, forecastScreenQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedForecastScreens, filteredForecastScreens.Count(), forecastScreenQueryparameter.PageNumber, forecastScreenQueryparameter.PageSize);

       


        }


        public async Task<ForecastScreen> GetForecastData(ForecastIdDetails forecastIdDetails)
        {
            var query = Builders<ForecastScreen>.Filter.Eq(i => i.SkillGroupId, forecastIdDetails.SkillGroupId)
                & Builders<ForecastScreen>.Filter.Eq(i => i.SkillGroupId, forecastIdDetails.SkillGroupId);

            return await FindByIdAsync(query);
        }
        public async Task<ForecastScreen> GetForecastDataID(int skillGroupId,string forecastDate)
        {
            var query = Builders<ForecastScreen>.Filter.Eq(i => i.SkillGroupId,skillGroupId) &
                Builders<ForecastScreen>.Filter.Eq(i => i.Date, forecastDate);
          

            return await FindByIdAsync(query);
        }

        public async Task<ForecastScreen> GetForecastScreenBySkillGroupId(ForecastIdDetails forecastIdDetails)
        {
            var query = Builders<ForecastScreen>.Filter.Eq(i => i.SkillGroupId, forecastIdDetails.SkillGroupId)
                & Builders<ForecastScreen>.Filter.Eq(i => i.Date, forecastIdDetails.Date);

            return await FindByIdAsync(query);
        }



        private IQueryable<ForecastScreen> FilterForecastscreen(IQueryable<ForecastScreen> forecastScreens, ForeCastScreenQueryParameter forecastScreenQueryparameter)
        {
            if (!forecastScreens.Any())
            {
                return forecastScreens;
            }

            if (!string.IsNullOrWhiteSpace(forecastScreenQueryparameter.SearchKeyword))
            {
                forecastScreens =  forecastScreens.Where(o => o.SkillGroupId.Equals(forecastScreenQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            return forecastScreens;
        }

        public void CreateForecastData(ForecastScreen forecastDataRequest)
        {
            InsertOneAsync(forecastDataRequest);
        }


        public void CreateMultipleForecastData(List<ForecastScreen> forecastDataRequest)
        {
            InsertManyAsync(forecastDataRequest);
        }

        public void UpdateForecastData(ForecastScreen updateForecastData)
        {


            ReplaceOneAsync(updateForecastData);
          
        }

    }

}
