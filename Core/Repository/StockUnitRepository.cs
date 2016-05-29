using System.Collections.Generic;
using Core.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace Core.Repository
{
    public class StockUnitRepository : Repository<StockUnit>
    {
        public StockUnit GetByIdFull(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<StockUnit>(id);
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

        public IList<StockUnit> GetAllFull(bool initLists)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<StockUnit>().List();
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Status);
                    NHibernateUtil.Initialize(item.Card);
                    NHibernateUtil.Initialize(item.Owner);
                    if (initLists)
                        NHibernateUtil.Initialize(item.UnitList);
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
    }
}
