using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Hypesoft.Infrastructure.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var settings = configuration.GetSection("MongoSettings");
            var connectionString = settings["ConnectionString"];
            var databaseName = settings["DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) =>
            _database.GetCollection<T>(name);
    }
}
