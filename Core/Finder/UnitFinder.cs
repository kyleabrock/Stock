using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Finder
{
    public class UnitFinder : IFinder
    {
        public DetachedCriteria Criteria
        {
            get { return _criteria; }
        }

        public void CreateCriteria(string searchString)
        {
            _criteria = DetachedCriteria.For<Unit>();

            _criteria.CreateAlias("UnitType", "type")
                     .CreateAlias("StockUnit", "stockUnit");

            ICriterion resultCriterion = AddUnitPropertiesRestrictions(searchString);
            resultCriterion = Restrictions.Or(resultCriterion, AddUnitTypeRestrictions(searchString));
            resultCriterion = Restrictions.Or(resultCriterion, AddStockUnitRestrictions(searchString));
            
            _criteria.Add(resultCriterion);
        }

        private DetachedCriteria _criteria;

        private ICriterion AddUnitPropertiesRestrictions(string searchString)
        {
            ICriterion manufacture = Restrictions.Like("Manufacture", searchString, MatchMode.Anywhere);
            ICriterion model = Restrictions.Like("ModelName", searchString, MatchMode.Anywhere);
            ICriterion serial = Restrictions.Like("Serial", searchString, MatchMode.Anywhere);

            ICriterion result = Restrictions.Or(manufacture, model);
            result = Restrictions.Or(result, serial);

            return result;
        }

        private ICriterion AddUnitTypeRestrictions(string searchString)
        {
            ICriterion result = Restrictions.Like("type.TypeName", searchString, MatchMode.Anywhere);
            return result;
        }

        private ICriterion AddStockUnitRestrictions(string searchString)
        {
            ICriterion result = Restrictions.Like("stockUnit.StockNumber", searchString, MatchMode.Anywhere);
            return result;
        }
    }
}
