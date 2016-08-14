using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Finder;

namespace Stock.Core.Repository
{
    public class StockUnitRepository : Repository<StockUnit>, IComplexFilterRepository<StockUnit>
    {
        public StockUnit GetById(int id, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<StockUnit>(id);
                if (!eagerLoading)
                    return result;

                NHibernateUtil.Initialize(result.Status);
                NHibernateUtil.Initialize(result.Card);
                NHibernateUtil.Initialize(result.Owner);
                NHibernateUtil.Initialize(result.UnitList);
                foreach (var unit in result.UnitList)
                {
                    NHibernateUtil.Initialize(unit.UnitType);
                }

                return result;
            }
        }

        public IList<StockUnit> GetAll(Expression<Func<StockUnit, object>> orderByPath, bool asc, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.QueryOver<StockUnit>().OrderBy(orderByPath);
                var result = asc ? query.Asc.List() : query.Desc.List();

                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Status);
                    NHibernateUtil.Initialize(item.Card);
                    NHibernateUtil.Initialize(item.Owner);
                    if (eagerLoading)
                        NHibernateUtil.Initialize(item.UnitList);
                }

                return result;
            }
        }

        public IList<StockUnit> GetAllOrdered()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<StockUnit>().OrderBy(x => x.StockNumber).Asc.List();
            }
        }

        public IList<StockUnit> GetAllByComplexFilter(IFilterBase filter)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var mainCriteria = session.CreateCriteria<StockUnit>();
                var stockUnitFilter = filter as StockUnitFilter;
                if (stockUnitFilter != null)
                {
                    if (stockUnitFilter.Card != null)
                        mainCriteria.CreateCriteria("Card").Add(Restrictions.Eq("Id", stockUnitFilter.Card.Id));
                    if (stockUnitFilter.Owner != null)
                        mainCriteria.CreateCriteria("Owner").Add(Restrictions.Eq("Id", stockUnitFilter.Owner.Id));
                    if (stockUnitFilter.Status != null)
                        mainCriteria.CreateCriteria("Status").Add(Restrictions.Eq("Id", stockUnitFilter.Status.Id));
                }
                var result = mainCriteria.List<StockUnit>();
                
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Status);
                    NHibernateUtil.Initialize(item.Card);
                    NHibernateUtil.Initialize(item.Owner);
                }

                return result;
            }
        }

        public IList<StockUnit> Find(StockUnitFinder finder)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var criteria = finder.Criteria ?? DetachedCriteria.For<StockUnit>();
                
                var result = criteria.GetExecutableCriteria(session).List<StockUnit>();
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Status);
                    NHibernateUtil.Initialize(item.Card);
                    NHibernateUtil.Initialize(item.Owner);
                }

                return result;
            }
        }

        public IList<StockUnit> GetFromDefaultCard()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<StockUnit>()
                    .CreateCriteria("Card")
                    .Add(Restrictions.Eq("IsDefault", true))
                    .List<StockUnit>();
            }
        }

        public IList<StockUnit> GetFromCard(int cardId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session
                    .CreateCriteria<StockUnit>()
                    .CreateCriteria("Card")
                    .Add(Restrictions.Eq("Id", cardId))
                    .List<StockUnit>();

                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Status);
                    NHibernateUtil.Initialize(item.Card);
                    NHibernateUtil.Initialize(item.Owner);
                    NHibernateUtil.Initialize(item.UnitList);
                    foreach (var unit in item.UnitList)
                        NHibernateUtil.Initialize(unit.UnitType);
                }

                return result;
            }
        }
    }
}
