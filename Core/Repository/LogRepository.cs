using System.Collections.Generic;
using NHibernate;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class LogRepository : Repository<Log>
    {
        public IList<Log> GetAllByUserId(int userId, int limit)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.QueryOver<Log>()
                    .Where(x => x.UserId == userId);
                
                if (limit > 0)
                    query.Take(limit);

                return query.List();
            }
        }
    }
}
