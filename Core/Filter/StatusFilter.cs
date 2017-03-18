using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class StatusFilter : FilterBase
    {
        public StatusFilter() : base()
        {
            
        }

        public override void CreateFilter()
        {
            if (string.IsNullOrEmpty(SearchString)) return;

            Criteria = DetachedCriteria.For<Account>()
                .Add(Restrictions.Or(
                Restrictions.Like("StatusName", SearchString, MatchMode.Anywhere),
                Restrictions.Like("Comments", SearchString, MatchMode.Anywhere)));
        }
    }
}
