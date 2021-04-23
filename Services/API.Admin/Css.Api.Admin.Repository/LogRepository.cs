using AutoMapper;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Log;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Repository
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        private readonly IMapper _mapper;
        public LogRepository(
        AdminContext repositoryContext,
        IMapper mapper)
        : base(repositoryContext)
        {
            _mapper = mapper;
        }

        public void CreateLog(Log log)
        {

            
            Create(log);
        }

    }
}
