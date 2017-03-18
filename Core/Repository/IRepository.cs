using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stock.Core.Domain;
using Stock.Core.Filter;

namespace Stock.Core.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        IList<T> GetAll();
        IList<T> GetAll(Expression<Func<T, object>> orderByPath, bool asc = true);
        T GetById(int id);
        IList<T> GetAllAsTableView();
        IList<T> Find(IFilter filter);

        void Save(T arg);
        void Save(IEnumerable<T> arg);
        void Add(T arg);
        void Add(IEnumerable<T> arg);
        void Update(T arg);
        void Update(IEnumerable<T> arg);
        void Delete(T arg);
        void Delete(IEnumerable<T> arg);
    }
}
