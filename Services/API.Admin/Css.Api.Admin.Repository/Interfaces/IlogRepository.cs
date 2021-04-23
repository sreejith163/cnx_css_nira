using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ILogRepository
    {
        void CreateLog(Log createLog);
    }
}
