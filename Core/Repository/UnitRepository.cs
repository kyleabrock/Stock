using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter;
using Stock.Core.Finder;

namespace Stock.Core.Repository
{
    public class UnitRepository : Repository<Unit>, IComplexFilterRepository<Unit>
    {
        public Unit GetById(int id, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<Unit>(id);
                if (!eagerLoading)
                    return result;

                NHibernateUtil.Initialize(result.UnitType);
                NHibernateUtil.Initialize(result.StockUnit.StockNumber);

                return result;
            }
        }

        public IList<Unit> GetAll(Expression<Func<Unit, object>> orderByPath, bool asc, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.QueryOver<Unit>().OrderBy(orderByPath);
                var result = asc ? query.Asc.List() : query.Desc.List();

                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.UnitType.TypeName);
                    NHibernateUtil.Initialize(item.StockUnit.StockNumber);
                }

                return result;
            }
        }

        public IList<Unit> GetAllByComplexFilter(IFilterBase filter)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var mainCriteria = session.CreateCriteria<Unit>();
                var unitFilter = filter as UnitFilter;
                if (unitFilter != null)
                {
                    if (unitFilter.Manufacture != null)
                        mainCriteria.Add(Restrictions.Eq("Manufacture", unitFilter.Manufacture));
                    if (unitFilter.ModelName != null)
                        mainCriteria.Add(Restrictions.Eq("ModelName", unitFilter.ModelName));
                    if (unitFilter.UnitType != null)
                        mainCriteria.CreateCriteria("UnitType").Add(Restrictions.Eq("Id", unitFilter.UnitType.Id));
                    if (unitFilter.Owner != null)
                    {
                        mainCriteria.CreateCriteria("StockUnit")
                                    .CreateCriteria("Owner")
                                    .Add(Restrictions.Eq("Id", unitFilter.Owner.Id));
                    }
                }

                var result = mainCriteria.List<Unit>();
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.UnitType.TypeName);
                    NHibernateUtil.Initialize(item.StockUnit.StockNumber);
                }

                return result;
            }
        }

        public IList<Unit> Find(UnitFinder finder)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var criteria = finder.Criteria ?? DetachedCriteria.For<Unit>();
                
                var result = criteria.GetExecutableCriteria(session).List<Unit>();
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.UnitType);
                    NHibernateUtil.Initialize(item.StockUnit.StockNumber);
                }

                return result;
            }
        }

        public IList<Unit> GetAllFull(int limit)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Unit>().Take(limit).List();
                foreach (var item in result)
                    NHibernateUtil.Initialize(item.UnitType.TypeName);

                return result;
            }
        }

        public IList<string> GetManufactureList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Unit>()
                    .Select(x => x.Manufacture)
                    .OrderBy(x => x.Manufacture)
                    .Asc.List<string>();

                return new List<string>(result.Distinct());
            }
        }

        public IList<string> GetModelList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Unit>()
                    .Select(x => x.ModelName)
                    .OrderBy(x => x.ModelName)
                    .Asc.List<string>();

                return new List<string>(result.Distinct());
            }
        }
    }
}
