using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Finder
{
    public class StockUnitFinder : IFinder
    {
        public DetachedCriteria Criteria { get; private set; }
        
        public void CreateCriteria(string searchString)
        {
            Criteria = DetachedCriteria.For<StockUnit>();

            Criteria.CreateAlias("Owner", "owner")
                    .CreateAlias("Card", "card");

            ICriterion resultCriterion = GetStockUnitCriterion(searchString);
            resultCriterion = Restrictions.Or(resultCriterion, GetCardCriterion(searchString));
            resultCriterion = Restrictions.Or(resultCriterion, GetOwnerCriterion(searchString));

            Criteria.Add(resultCriterion);
        }

        private ICriterion GetStockUnitCriterion(string searchString)
        {
            ICriterion stockNumber = Restrictions.Like("StockNumber", searchString, MatchMode.Anywhere);
            ICriterion stockName = Restrictions.Like("StockName", searchString, MatchMode.Anywhere);
            //ICriterion creationDate = Restrictions.Like("CreationDate", searchString, MatchMode.Anywhere);
            ICriterion comments = Restrictions.Like("Comments", searchString, MatchMode.Anywhere);

            ICriterion result = Restrictions.Or(stockNumber, stockName);
            //result = Restrictions.Or(result, creationDate);
            result = Restrictions.Or(result, comments);

            return result;
        }

        private ICriterion GetOwnerCriterion(string searchString)
        {
            ICriterion result = Restrictions.Like("owner.Name.DisplayName", searchString, MatchMode.Anywhere);
            return result;
        }

        private ICriterion GetCardCriterion(string searchString)
        {
            ICriterion result = Restrictions.Like("card.CardNumber", searchString, MatchMode.Anywhere);
            return result;
        }
    }
}
