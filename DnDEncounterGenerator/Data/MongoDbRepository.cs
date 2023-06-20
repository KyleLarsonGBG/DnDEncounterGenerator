using DnDEncounterGenerator.Configuration;
using DnDEncounterGenerator.Data.Models.Entity;
using MongoDB.Driver;

namespace DnDEncounterGenerator.Data
{
    public class MongoDbRepository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TEntity> _collection;

        public MongoDbRepository(
            IMongoClient mongoClient, 
            IEncounterGeneratorConfiguration config)
        {
            _mongoClient = mongoClient;
            var databaseName = config.DatabaseName;
            var collectionName = config.CollectionName;
            _database = _mongoClient.GetDatabase(databaseName);
            _collection = _database.GetCollection<TEntity>(collectionName);
        }

        public async Task<bool> AddEntity(TEntity entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                return true;
            }
            catch
            {
                //log error
                return false;
            }
        }

        public async Task<bool> AddEntities(IEnumerable<TEntity> entities)
        {
            try
            {
                await _collection.InsertManyAsync(entities);
                return true;
            }
            catch
            {
                //log error
                return false;
            }
        }

        public async Task<bool> UpdateEntity(TEntity entity)
        {
            try
            {
                var filter = Builders<TEntity>.Filter
                    .Eq(e => e.Id, entity.Id);

                await _collection.ReplaceOneAsync(filter, entity);

                return true;
            }
            catch
            {
                //log error
                return false;
            }
        }

        public async Task<IEnumerable<TEntity>> GetEntities(IQueryParameters<TEntity> queryParams)
        {
            var filter = Builders<TEntity>.Filter
                .Where(queryParams.BuildExpression());

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<object> GetEntity(Guid entityId)
        {
            var filter = Builders<TEntity>.Filter
                .Eq(e => e.Id, entityId); ;

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> ListEntities()
        {
            var filter = Builders<TEntity>.Filter
                .Empty;

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> DeleteEntity(Guid entityId)
        {
            try
            {
                var filter = Builders<TEntity>.Filter
                    .Eq(e => e.Id, entityId);

                await _collection.DeleteOneAsync(filter);

                return true;
            }
            catch
            {
                //log error
                return false;
            }
        }
    }
}
