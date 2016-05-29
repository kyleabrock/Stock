using System.Collections.Generic;
using Core.Domain;
using NHibernate;

namespace Core.Repository
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
