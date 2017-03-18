using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter.FilterParams;

namespace Stock.Core.Filter
{
    public class CardFilter : FilterBase
    {
        public CardFilter() : base()
        {

        }

        public CardFilter(CardFilterParams filterParams)
        {
            if (filterParams != null)
                FilterParams = filterParams;
        }

        public override void CreateFilter()
        {
            var filterParams = FilterParams as CardFilterParams;
            if (filterParams == null) return;

            Criteria = DetachedCriteria.For<Card>();
            Criteria.CreateAlias("Staff", "staff");

            var staff = filterParams.Staff.Cast<EntityBase>().ToList();
            var department = filterParams.Department.ToList();

            var staffCriterion = CreateCriterion(staff, "staff");
            var departmentCriterion = CreateCriterion(department, "staff", "Department");

            if (!string.IsNullOrEmpty(SearchString))
                SetSearchString(SearchString);

            ICriterion result = staffCriterion;
            if (result == null && departmentCriterion != null)
                result = departmentCriterion;
            if (result != null && departmentCriterion != null)
                result = Restrictions.And(departmentCriterion, result);

            if (result == null && SearchStringCriterion != null)
                result = SearchStringCriterion;
            if (result != null && SearchStringCriterion != null)
                result = Restrictions.And(SearchStringCriterion, result);

            if (result != null) Criteria.Add(result);
        }

        private void SetSearchString(string searchString)
        {
            ICriterion cardNumber = Restrictions.Like("CardNumber", searchString, MatchMode.Anywhere);
            ICriterion cardName = Restrictions.Like("CardName", searchString, MatchMode.Anywhere);
            ICriterion comments = Restrictions.Like("Comments", searchString, MatchMode.Anywhere);

            ICriterion displayName = Restrictions.Like("staff.Name.DisplayName", searchString, MatchMode.Anywhere);

            ICriterion result = Restrictions.Or(cardNumber, cardName);
            result = Restrictions.Or(result, comments);
            result = Restrictions.Or(result, displayName);

            SearchStringCriterion = result;
        }
    }
}
