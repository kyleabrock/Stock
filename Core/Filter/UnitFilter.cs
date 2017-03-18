using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter.FilterParams;

namespace Stock.Core.Filter
{
    public class UnitFilter : FilterBase
    {
        public UnitFilter() : base()
        {
            
        }

        public UnitFilter(UnitFilterParams filterParams)
        {
            if (filterParams != null)
                FilterParams = filterParams;
        }

        public override void CreateFilter()
        {
            var filterParams = FilterParams as UnitFilterParams;
            if (filterParams == null) return;
            
            Criteria = DetachedCriteria.For<Unit>();
            Criteria.CreateAlias("UnitType", "type")
                     .CreateAlias("StockUnit", "stockUnit")
                     .CreateAlias("StockUnit.Status", "status")
                     .CreateAlias("StockUnit.Owner", "owner");

            var unitType = filterParams.UnitType.Cast<EntityBase>().ToList();
            var owners = filterParams.Owner.Cast<EntityBase>().ToList();
            var statuses = filterParams.Status.Cast<EntityBase>().ToList();
            var models = filterParams.ModelName;
            var manufactures = filterParams.Manufacture;

            var ownerCriterion = CreateCriterion(owners, "owner");
            var statusesCriterion = CreateCriterion(statuses, "owner");
            var unitTypeCriterion = CreateCriterion(unitType, "type");
            var modelsCriterion = CreateCriterion(models, "type", "ModelName");
            var manufactureCriterion = CreateCriterion(manufactures, "type", "Manufacture");

            if (!string.IsNullOrEmpty(SearchString))
                SetSearchString(SearchString);

            ICriterion result = ownerCriterion;
            if (result == null && statusesCriterion != null)
                result = statusesCriterion;
            if (result != null && statusesCriterion != null)
                result = Restrictions.And(statusesCriterion, result);

            if (result == null && unitTypeCriterion != null)
                result = unitTypeCriterion;
            if (result != null && unitTypeCriterion != null)
                result = Restrictions.And(unitTypeCriterion, result);

            if (result == null && modelsCriterion != null)
                result = modelsCriterion;
            if (result != null && modelsCriterion != null)
                result = Restrictions.And(modelsCriterion, result);

            if (result == null && manufactureCriterion != null)
                result = manufactureCriterion;
            if (result != null && manufactureCriterion != null)
                result = Restrictions.And(manufactureCriterion, result);

            if (result == null && SearchStringCriterion != null)
                result = SearchStringCriterion;
            if (result != null && SearchStringCriterion != null)
                result = Restrictions.And(SearchStringCriterion, result);

            if (result != null) Criteria.Add(result);
        }

        private void SetSearchString(string searchString)
        {
            ICriterion manufacture = Restrictions.Like("Manufacture", searchString, MatchMode.Anywhere);
            ICriterion model = Restrictions.Like("ModelName", searchString, MatchMode.Anywhere);
            ICriterion serial = Restrictions.Like("Serial", searchString, MatchMode.Anywhere);
            ICriterion comments = Restrictions.Like("Comments", searchString, MatchMode.Anywhere);

            ICriterion typeName = Restrictions.Like("type.TypeName", searchString, MatchMode.Anywhere);
            ICriterion stockNumber = Restrictions.Like("stockUnit.StockNumber", searchString, MatchMode.Anywhere);

            ICriterion result = Restrictions.Or(manufacture, model);
            result = Restrictions.Or(result, serial);
            result = Restrictions.Or(result, comments);
            result = Restrictions.Or(result, typeName);
            result = Restrictions.Or(result, stockNumber);

            SearchStringCriterion = result;
        }
    }
}
