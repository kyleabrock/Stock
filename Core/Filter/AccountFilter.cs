using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class AccountFilter : FilterBase
    {
        public override void CreateFilter()
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                Criteria = DetachedCriteria.For<Account>()
                    .Add(Restrictions.Like("Login", SearchString, MatchMode.Anywhere));
            }
        }
    }
}
