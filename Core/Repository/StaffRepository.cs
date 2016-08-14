using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Stock.Core.Domain;

namespace Stock.Core.Repository
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

        public IList<string> GetDepartments()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Staff>()
                    .Select(x => x.Department)
                    .OrderBy(x => x.Department).Asc
                    .List<string>();
                
                return new List<string>(result.Distinct());
            }
        }
    }
}
