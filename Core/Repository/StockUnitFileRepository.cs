using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class StockUnitFileRepository : Repository<StockUnitFile>
    {
        public IList<StockUnitFile> GetByStockUnitId(StockUnit stockUnit)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.CreateCriteria<StockUnitFile>()
                                    .CreateCriteria("StockUnit")
                                    .Add(Restrictions.Eq("Id", stockUnit.Id))
                                    .List<StockUnitFile>();

                return result;
            }
        }
    }
}
