using Core.Domain;
using NHibernate;

namespace Core.Repository
{
    public class UnitRepository : Repository<Unit>
    {
        public Unit GetByIdFull(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<Unit>(id);
                NHibernateUtil.Initialize(result.UnitType);

                return result;
            }
        }
    }
}
