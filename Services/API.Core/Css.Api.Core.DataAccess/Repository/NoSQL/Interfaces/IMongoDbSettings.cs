namespace Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces
{
    public interface IMongoDbSettings
    {
        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        string ConnectionString { get; set; }
    }
}
