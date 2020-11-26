using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using System.Threading.Tasks;

namespace Css.Api.Core.DataAccess.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly IMongoContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
