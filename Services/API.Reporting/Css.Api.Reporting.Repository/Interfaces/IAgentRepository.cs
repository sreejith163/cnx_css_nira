using Css.Api.Reporting.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Repository.Interfaces
{
    public interface IAgentRepository
    {
        void UpsertAsync(List<Agent> agents);
    }
}
