using System.Collections.Generic;
using NHibernate;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class OwnerRepository : Repository<Owner>
    {
        public IList<Owner> GetAllOrdered()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Owner>()
                    .OrderBy(x => x.Name.DisplayName)
                    .Asc
                    .List();
            }
        }
    }
}
