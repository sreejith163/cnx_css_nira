using AutoMapper;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
  public class AuthRepository : GenericRepository<NonSsoModel>, IAuthRepository
    {
        private readonly IMapper _mapper;
        public AuthRepository(
        AdminContext repositoryContext,
        IMapper mapper)
        : base(repositoryContext)
        {
            _mapper = mapper;
        }
        public async Task<NonSsoModel> Login(CredentialsViewModel credentialsViewModel)
        {
            var Auth = FindByCondition(x => 
            x.Username == credentialsViewModel.UserName && 
            x.Password == credentialsViewModel.Password && 
            x.IsDeleted == false)
              
                .SingleOrDefault();

         


            return await Task.FromResult(Auth);
        }


        public async Task<NonSsoModel> GetUserDetails(CredentialsViewModel credentialsViewModel)
        {
            var Auth = FindByCondition(x =>
            x.Username == credentialsViewModel.UserName &&
            x.Password == credentialsViewModel.Password &&
            x.IsDeleted == false).SingleOrDefault();


            return await Task.FromResult(Auth);
        }
    }
}
