using AutoMapper;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.UserLanguage;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class UserLanguageRepository : GenericRepository<UserLanguagePreference>, IUserLanguageRepository
    {

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLanguageRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public UserLanguageRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        public void CreateUserLanguagePreference(UserLanguagePreference userLanguagePreference)
        {
            Create(userLanguagePreference);
        }

        public async Task<UserLanguagePreference> GetUserLanguagePreference(UserLanguageEmployeeIdDetails userLanguageEmployeeIdDetails)
        {
            var userLanguage = FindByCondition(x => x.EmployeeId == userLanguageEmployeeIdDetails.EmployeeId)
                .SingleOrDefault();

            return await Task.FromResult(userLanguage);
        }

        public void UpdateUserLanguagePreference(UserLanguagePreference userLanguagePreference)
        {
            Update(userLanguagePreference);
        }
    }
}
