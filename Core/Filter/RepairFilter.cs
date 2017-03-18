using System.Collections.Generic;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter.FilterParams;

namespace Stock.Core.Filter
{
    public class RepairFilter : FilterBase
    {
        public RepairFilter() : base()
        {
            
        }

        public RepairFilter(RepairFilterParams filterParams)
        {
            if (filterParams != null)
                FilterParams = filterParams;
        }

        public override void CreateFilter()
        {
            var filterParams = FilterParams as RepairFilterParams;
            if (filterParams == null) return;

            Criteria = DetachedCriteria.For<Repair>();
            Criteria.CreateAlias("User", "user");

            var users = filterParams.User as IEnumerable<EntityBase>;
            
            var usersCriterion = CreateCriterion(users, "user");

            if (!string.IsNullOrEmpty(SearchString))
                SetSearchString(SearchString);

            ICriterion result = usersCriterion;
            if (result == null && SearchStringCriterion != null)
                result = SearchStringCriterion;
            if (result != null && SearchStringCriterion != null)
                result = Restrictions.And(SearchStringCriterion, result);

            if (result != null) Criteria.Add(result);
        }

        private void SetSearchString(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return;

            ICriterion defect = Restrictions.Like("Defect", searchString, MatchMode.Anywhere);
            ICriterion repairResult = Restrictions.Like("Result", searchString, MatchMode.Anywhere);
            ICriterion user = Restrictions.Like("user.Name.DisplayName", searchString, MatchMode.Anywhere);
            ICriterion comments = Restrictions.Like("Comments", searchString, MatchMode.Anywhere);

            ICriterion result = Restrictions.Or(defect, repairResult);
            result = Restrictions.Or(result, user);
            result = Restrictions.Or(result, comments);

            SearchStringCriterion = result;
        }
    }
}
