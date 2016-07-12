using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stock.Core.Domain;
using Stock.Core.Filter;

namespace Stock.Core.Repository
{
    public interface IComplexFilterRepository<T> where T : EntityBase
    {
        IList<T> GetAllByComplexFilter(IFilterBase filter);
        IList<T> GetAll(Expression<Func<T, object>> orderByPath, bool asc, bool eagerLoading);
    }
}
