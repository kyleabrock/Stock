using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using Stock.Core.Domain;

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

        protected override void InitializeTableValues(IEnumerable<Unit> items)
        {
            foreach (var item in items)
            {
                NHibernateUtil.Initialize(item.UnitType);
                NHibernateUtil.Initialize(item.StockUnit.StockNumber);
                NHibernateUtil.Initialize(item.StockUnit.Status.StatusName);
                NHibernateUtil.Initialize(item.StockUnit.Status.StatusType);
            }
        }
    }
}
