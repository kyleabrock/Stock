using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class LogFilter : FilterBase
    {
        public LogFilter() : base()
        {
            
        }

        public override void CreateFilter()
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                Criteria = DetachedCriteria.For<Log>()
                    .Add(Restrictions
                    .Or(Restrictions.Like("UserName", SearchString, MatchMode.Anywhere),
                    Restrictions.Like("Message", SearchString, MatchMode.Anywhere)));
            }
        }
    }
}
