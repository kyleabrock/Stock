using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class OwnerFilter : FilterBase
    {
        public OwnerFilter() : base()
        {
            
        }

        public override void CreateFilter()
        {
            if (string.IsNullOrEmpty(SearchString)) return;
            
            Criteria = DetachedCriteria.For<Owner>();
            var lastName = Restrictions.Like("Name.LastName", SearchString, MatchMode.Anywhere);
            var firstName = Restrictions.Like("Name.FirstName", SearchString, MatchMode.Anywhere);
            var patronymic = Restrictions.Like("Name.Patronymic", SearchString, MatchMode.Anywhere);
            var displayName = Restrictions.Like("Name.DisplayName", SearchString, MatchMode.Anywhere);
            var department = Restrictions.Like("Name.Department", SearchString, MatchMode.Anywhere);
            var comments = Restrictions.Like("Name.Comments", SearchString, MatchMode.Anywhere);
            var lastOrFirst = Restrictions.Or(lastName, firstName);
            var patronymicOrDisplay = Restrictions.Or(patronymic, displayName);
            var departmentOrComments = Restrictions.Or(department, comments);

            Criteria.Add(Restrictions.Or(
                Restrictions.Or(lastOrFirst, patronymicOrDisplay), departmentOrComments));
        }
    }
}
