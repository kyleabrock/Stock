using System.Collections;
using System.Collections.Generic;
using Core.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace Core.Repository
{
    public class CardRepository : Repository<Card>
    {
        public Card GetByIdFull(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<Card>(id);
                NHibernateUtil.Initialize(result.Staff);
                NHibernateUtil.Initialize(result.StockUnitList);
                
                return result;
            }
        }

        public IList<Card> GetAllFull(bool initLists)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Card>().List();
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.Staff);
                    if (initLists)
                        NHibernateUtil.Initialize(item.StockUnitList);
                }

                return result;
            }
        }

        public Card GetDefaultCard()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList resultList = session.CreateCriteria<Card>()
                    .Add(Restrictions.Eq("IsDefault", true)).List();

                if (resultList.Count > 0)
                {
                    var result = (Card) resultList[0];
                    NHibernateUtil.Initialize(result.Staff);
                    NHibernateUtil.Initialize(result.StockUnitList);

                    return result;
                }

                return new Card();
            }
        }
    }
}
