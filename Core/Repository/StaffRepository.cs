using System.Collections.Generic;
using Core.Domain;
using NHibernate;

namespace Core.Repository
{
    public class StaffRepository : Repository<Staff>
    {
        public IList<Staff> GetAllOrdered()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Staff>()
                    .OrderBy(x => x.Name.DisplayName)
                    .Asc
                    .List();
            }
        }
    }
}
