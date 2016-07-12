using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;

namespace Stock.Core.Repository
{
    public class AccountRepository : Repository<Account>
    {
        public Account GetByLogin(string login)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.CreateCriteria<Account>().Add(Restrictions.Eq("Login", login)).List<Account>();
                if (result.Count > 0)
                    return result[0];
                return new Account();
            }
        }
    }
}
