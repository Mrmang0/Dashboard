using Dashboard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dashboard.Application.Repository
{
    public interface IRepository<TModel> where TModel : BaseModel
    {
        public Task<TModel> GetOneAsync(Guid id);
        public IQueryable<TModel> AsQueryable();
        public Task<IEnumerable<TModel>> AsEnumerable();
        public Task<TModel> SaveAsync(TModel model);
        public Task RemoveAsync(Guid id);
        public Task RemoveAsync(TModel model);
        public Task RemoveAsync(Expression<Func<TModel, bool>> predicate);
        public Task<IEnumerable<TModel>> GetAllAsync();
        public Task<IEnumerable<TModel>> GetAllAsync(Expression<Func<TModel, bool>> predicate);
    }
}
