using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;

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

        protected override void InitializeTableValues(IEnumerable<Card> items)
        {
            foreach (var item in items)
            {
                NHibernateUtil.Initialize(item.Staff);
                NHibernateUtil.Initialize(item.StockUnitList);
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
