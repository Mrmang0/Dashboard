using Dashboard.Application.Repository;
using Dashboard.Domain.Models;
using Dashboard.Infrastrucure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Infrastrucure.Repositories
{
    public class MongoDbRepository<TModel> : IRepository<TModel> where TModel : BaseModel
    {
        protected string _collectionName;
        protected string _databaseName;
        protected MongoClient _mongo;

        public IMongoCollection<TModel> Collection => Database.GetCollection<TModel>(_collectionName);
        protected IMongoDatabase Database => _mongo.GetDatabase(_databaseName);

        public MongoDbRepository(IOptions<MongoDBSettings> options)
        {
            _mongo = new MongoClient(options.Value.ConnectionString);
            _databaseName = MongoUrl.Create(options.Value.ConnectionString).DatabaseName;
            _collectionName = typeof(TModel).Name;
            if (!BsonClassMap.IsClassMapRegistered(typeof(TModel)))
            {
                BsonClassMap.RegisterClassMap<TModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }

        public IEnumerable<TModel> AsEnumerable()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TModel> AsQueryable()
        {
            return Collection.AsQueryable();
        }

        public TModel GetOne(Guid id)
        {
            return Collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public void Remove(Expression<Func<TModel,bool>> predicate)
        {
            Collection.DeleteOne(predicate);
        }

        public void Remove(TModel model)
        {
            Collection.DeleteOne(x => x.Id == model.Id);
        }
        public void Remove(Guid id)
        {
            Collection.DeleteOne(x => x.Id == id);

        }

        public TModel Save(TModel model)
        {
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                model.Created = DateTime.UtcNow;
            }

            model.Updated = DateTime.UtcNow;
            var filter = Builders<TModel>.Filter.Eq(x => x.Id, model.Id);
            this.Collection.ReplaceOne(filter, model, new ReplaceOptions { IsUpsert = true });
            return model;
        }

        public async Task<TModel> GetOneAsync(Guid id)
        {
           return await (await Collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<TModel> SaveAsync(TModel model)
        {
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                model.Created = DateTime.UtcNow;
            }

            model.Updated = DateTime.UtcNow;
            var filter = Builders<TModel>.Filter.Eq(x => x.Id, model.Id);
            await Collection.ReplaceOneAsync(filter, model, new ReplaceOptions { IsUpsert = true });
            return model;
        }

        public async Task RemoveAsync(Guid id)
        {
            await this.Collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(TModel model)
        {
            await Collection.DeleteOneAsync(x => x.Id == model.Id);
        }

        public async Task RemoveAsync(Expression<Func<TModel, bool>> predicate)
        {
           await Collection.DeleteOneAsync(predicate);
        }

        Task<IEnumerable<TModel>> IRepository<TModel>.AsEnumerable()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await (await Collection.FindAsync(x => true)).ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await (await Collection.FindAsync(predicate)).ToListAsync();

        }
    }
}
