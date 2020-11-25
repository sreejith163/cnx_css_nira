using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;

namespace Css.Api.Core.DataAccess.Repository.NoSQL
{
    public class MongoDbSettings : IMongoDbSettings
    {
        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
