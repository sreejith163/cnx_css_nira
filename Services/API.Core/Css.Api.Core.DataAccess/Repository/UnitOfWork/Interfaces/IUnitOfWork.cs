using System;
using System.Threading.Tasks;

namespace Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <returns></returns>
        Task<bool> Commit();
    }
}
