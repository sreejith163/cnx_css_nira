using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Core.DataAccess.Repository.NoSQL
{
    public class MongoContext : IMongoContext
    {
        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        private IMongoDatabase Database { get; set; }

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        public IClientSessionHandle Session { get; set; }

        /// <summary>
        /// Gets or sets the mongo client.
        /// </summary>
        public MongoClient MongoClient { get; set; }

        /// <summary>
        /// The commands
        /// </summary>
        private readonly List<Func<Task>> _commands;

        /// <summary>
        /// The mongo database settings
        /// </summary>
        private readonly IMongoDbSettings _mongoDbSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoContext" /> class.
        /// </summary>
        /// <param name="mongoDbSettings">The mongo database settings.</param>
        public MongoContext(IMongoDbSettings mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings;
            _commands = new List<Func<Task>>();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChanges()
        {
            ConfigureMongo();
            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="func">The function.</param>
        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();
            return Database.GetCollection<T>(name);
        }

        /// <summary>
        /// Configures the mongo.
        /// </summary>
        private void ConfigureMongo()
        {
            if (MongoClient == null)
            {
                MongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
                Database = MongoClient.GetDatabase(_mongoDbSettings.DatabaseName);
            }
        }
    }
}
