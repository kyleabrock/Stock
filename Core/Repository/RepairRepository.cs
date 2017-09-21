using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class RepairRepository : Repository<Repair>, IComplexFilterRepository<Repair>
    {
        public Repair GetById(int id, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<Repair>(id);
                NHibernateUtil.Initialize(result.Unit);
                NHibernateUtil.Initialize(result.Unit.StockUnit.StockNumber);
                NHibernateUtil.Initialize(result.User);

                return result;
            }
        }

        public IList<Repair> GetAll(Expression<Func<Repair, object>> orderByPath, bool asc, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.QueryOver<Repair>().OrderBy(orderByPath);
                var result = asc ? query.Asc.List() : query.Desc.List();
                    
                if (!eagerLoading)
                    return result;

                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Unit);
                    NHibernateUtil.Initialize(item.Unit.StockUnit.StockNumber);
                    NHibernateUtil.Initialize(item.User);
                }

                return result;
            }
        }

        public IList<Repair> GetAllByStockUnit(StockUnit stockUnit)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                if (!stockUnit.IsNew)
                {
                    var result = session.CreateCriteria<Repair>()
                        .CreateCriteria("Unit")
                        .CreateCriteria("StockUnit")
                        .Add(Restrictions.Eq("Id", stockUnit.Id))
                        .List<Repair>();

                    foreach (var item in result)
                    {
                        NHibernateUtil.Initialize(item.Unit);
                        NHibernateUtil.Initialize(item.Unit.StockUnit.StockNumber);
                        NHibernateUtil.Initialize(item.User);
                    }

                    return result;
                }

                return new List<Repair>();
            }
        }
        
        protected override void InitializeTableValues(IEnumerable<Repair> items)
        {
            foreach (var item in items)
            {
                NHibernateUtil.Initialize(item.Unit);
                NHibernateUtil.Initialize(item.Unit.StockUnit.StockNumber);
                NHibernateUtil.Initialize(item.User);
            }
        }
    }
}
