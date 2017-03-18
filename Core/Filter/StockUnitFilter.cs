using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter.FilterParams;

namespace Stock.Core.Filter
{
    public class StockUnitFilter : FilterBase
    {
        public StockUnitFilter() : base()
        {
            
        }

        public StockUnitFilter(StockUnitFilterParams filterParams)
        {
            if (filterParams != null)
                FilterParams = filterParams;
        }

        public override void CreateFilter()
        {
            var filterParams = FilterParams as StockUnitFilterParams;
            if (filterParams == null) return;
            
            Criteria = DetachedCriteria.For<StockUnit>();

            Criteria.CreateAlias("Owner", "owner")
                    .CreateAlias("Card", "card")
                    .CreateAlias("Status", "status");

            var owners = filterParams.Owner.Cast<EntityBase>().ToList();
            var statuses = filterParams.Status.Cast<EntityBase>().ToList();
            var cards = filterParams.Card.Cast<EntityBase>().ToList();

            var ownerCriterion = CreateCriterion(owners, "owner");
            var statusesCriterion = CreateCriterion(statuses, "status");
            var cardCriterion = CreateCriterion(cards, "card");

            if (!string.IsNullOrEmpty(SearchString))
                SetSearchString(SearchString);
            
            ICriterion result = ownerCriterion;
            if (result == null && statusesCriterion != null) 
                result = statusesCriterion;
            if (result != null && statusesCriterion != null)
                result = Restrictions.And(statusesCriterion, result);
            
            if (result == null && cardCriterion != null)
                result = cardCriterion;
            if (result != null && cardCriterion != null)
                result = Restrictions.And(cardCriterion, result);
            
            if (result == null && SearchStringCriterion != null)
                result = SearchStringCriterion;
            if (result != null && SearchStringCriterion != null)
                result = Restrictions.And(SearchStringCriterion, result);

            if (result != null) Criteria.Add(result);
        }

        private void SetSearchString(string searchString)
        {
            ICriterion stockNumber = Restrictions.Like("StockNumber", searchString, MatchMode.Anywhere);
            ICriterion stockName = Restrictions.Like("StockName", searchString, MatchMode.Anywhere);
            ICriterion comments = Restrictions.Like("Comments", searchString, MatchMode.Anywhere);

            ICriterion ownerCriterion = Restrictions.Like("owner.Name.DisplayName", searchString, MatchMode.Anywhere);
            ICriterion cardCriterion = Restrictions.Like("card.CardNumber", searchString, MatchMode.Anywhere);

            ICriterion result = Restrictions.Or(stockNumber, stockName);
            result = Restrictions.Or(result, comments);
            result = Restrictions.Or(result, cardCriterion);
            result = Restrictions.Or(result, ownerCriterion);

            SearchStringCriterion = result;
        }
    }
}
