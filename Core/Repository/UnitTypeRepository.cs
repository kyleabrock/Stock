using System.Collections.Generic;
using NHibernate;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class UnitTypeRepository : Repository<UnitType>
    {
        public IList<UnitType> GetAllOrdered()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<UnitType>()
                    .OrderBy(x => x.TypeName)
                    .Asc
                    .List();
            }
        }
    }
}
