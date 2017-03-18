using NHibernate.Criterion;
using Stock.Core.Filter.FilterParams;

namespace Stock.Core.Filter
{
    public interface IFilter
    {
        DetachedCriteria Criteria { get; }
        
        string SearchString { get; set; }
        IFilterParams FilterParams { get; set; }
        void CreateFilter();
        void ClearFilter();
        string LastError { get; set; }
    }
}
