using Css.Api.Core.Common.Utilities.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interface;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private SchedulingContext repositoryContext { get; set; }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        private IClientRepository clientRepository { get; set; }

        /// <summary>
        /// The clients sort helper
        /// </summary>
        private readonly ISortHelper<Client> clientsSortHelper;

        /// <summary>
        /// The clients data shaper
        /// </summary>
        private readonly IDataShaper<Client> clientsDataShaper;

        /// <summary>
        /// Gets the stroies.
        /// </summary>
        public IClientRepository Clients
        {
            get
            {
                if (clientRepository == null)
                {
                    clientRepository = new ClientRepository(repositoryContext, clientsSortHelper, clientsDataShaper);
                }
                return clientRepository;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="clientsSortHelper">The clients sort helper.</param>
        /// <param name="clientsDataShaper">The clients data shaper.</param>
        public RepositoryWrapper(
            SchedulingContext repositoryContext,
            ISortHelper<Client> clientsSortHelper,
            IDataShaper<Client> clientsDataShaper)
        {
            this.repositoryContext = repositoryContext;
            this.clientsSortHelper = clientsSortHelper;
            this.clientsDataShaper = clientsDataShaper;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            return await repositoryContext.SaveChangesAsync() >= 0;
        }
    }
}
