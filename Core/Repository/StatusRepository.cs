using System.Collections.Generic;
using NHibernate;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class StatusRepository : Repository<Status>
    {
        public IList<Status> GetAllOrdered()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<Status>()
                    .OrderBy(x => x.StatusType)
                    .Asc
                    .List();
            }
        }
    }
}
