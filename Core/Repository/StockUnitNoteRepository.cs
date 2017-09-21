using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class StockUnitNoteRepository : Repository<StockUnitNote>
    {
        public IList<StockUnitNote> GetByStockUnit(StockUnit stockUnit)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.CreateCriteria<StockUnitNote>()
                                    .CreateCriteria("StockUnit")
                                    .Add(Restrictions.Eq("Id", stockUnit.Id))
                                    .List<StockUnitNote>();

                return result;
            }
        }
    }
}
