using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter;

namespace Stock.Core.Repository
{
    public class CardRepository : Repository<Card>, IComplexFilterRepository<Card>
    {
        public Card GetById(int id, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<Card>(id);
                if (!eagerLoading)
                    return result;
                NHibernateUtil.Initialize(result.Staff);
                NHibernateUtil.Initialize(result.StockUnitList);
                foreach (var stockUnit in result.StockUnitList)
                {
                    NHibernateUtil.Initialize(stockUnit.Status);
                    NHibernateUtil.Initialize(stockUnit.Owner);
                }

                return result;
            }
        }

        public IList<Card> GetAll(Expression<Func<Card, object>> orderByPath, bool asc, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.QueryOver<Card>().OrderBy(orderByPath);
                var result = asc ? query.Asc.List() : query.Desc.List();

                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Staff);
                    if (eagerLoading)
                        NHibernateUtil.Initialize(item.StockUnitList);
                }

                return result;
            }
        }

        public IList<Card> GetAllByComplexFilter(IFilterBase filter)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var mainCriteria = session.CreateCriteria<Card>();

                var cardFilter = filter as CardFilter;
                if (cardFilter != null)
                {
                    if (cardFilter.Staff != null && cardFilter.Department == null)
                        mainCriteria.CreateCriteria("Staff").Add(Restrictions.Eq("Id", cardFilter.Staff.Id));
                    if (cardFilter.Staff != null && cardFilter.Department != null)
                    {
                        mainCriteria.CreateCriteria("Staff")
                            .Add(Restrictions.Eq("Id", cardFilter.Staff.Id))
                            .Add(Restrictions.Eq("Department", cardFilter.Department));
                    }
                    if (cardFilter.Staff == null && cardFilter.Department != null)
                        mainCriteria.CreateCriteria("Staff").Add(Restrictions.Eq("Department", cardFilter.Department));
                }

                var result = mainCriteria.List<Card>();
                foreach (var item in result)
                    NHibernateUtil.Initialize(item.Staff);

                return result;
            }
        }

        public Card GetDefaultCard()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList resultList = session.CreateCriteria<Card>()
                    .Add(Restrictions.Eq("IsDefault", true)).List();

                if (resultList.Count > 0)
                {
                    var result = (Card) resultList[0];
                    NHibernateUtil.Initialize(result.Staff);
                    NHibernateUtil.Initialize(result.StockUnitList);

                    return result;
                }

                return new Card();
            }
        }
    }
}
