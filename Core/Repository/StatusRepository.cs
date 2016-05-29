using System.Collections.Generic;
using Core.Domain;
using NHibernate;

namespace Core.Repository
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
