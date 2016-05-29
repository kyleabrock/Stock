using System.Collections.Generic;
using Core.Domain;
using NHibernate;

namespace Core.Repository
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
