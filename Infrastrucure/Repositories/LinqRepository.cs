using Dashboard.Application;
using Dashboard.Application.Repository;
using Dashboard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dashboard.Infrastructure.Repositories
{
    //public class LinqRepository<TModel>/* : IRepository<TModel> where TModel : BaseModel*/
    //{
    //    public static readonly HashSet<TModel> LinqDB = new HashSet<TModel>();

    //    public IEnumerable<TModel> AsEnumerable()
    //    {
    //        return LinqDB.AsEnumerable();
    //    }

    //    public IQueryable<TModel> AsQueryable()
    //    {
    //        return LinqDB.AsQueryable();
    //    }

    //    public TModel GetOne(Guid id)
    //    {
    //        return LinqDB.FirstOrDefault(x => x.Id == id);
    //    }

    //    public TModel Save(TModel item)
    //    {
    //        var existingElement = GetOne(item.Id);
    //        if (existingElement != default)
    //        {
    //            existingElement = item;
    //        }
    //        else
    //        {
    //            LinqDB.Add(item);
    //        }

    //        return item;
    //    }

    //    public void Remove(Guid id)
    //    {
    //        var item = GetOne(id);
    //        LinqDB.Remove(item);
    //    }

    //    public void Remove(TModel model)
    //    {
    //        LinqDB.Remove(model);
    //    }

    //    public void Remove(Expression<Func<TModel, bool>> predicate)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
