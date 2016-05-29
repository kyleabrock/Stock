using System.Collections.Generic;
using Core.Domain;
using NHibernate;

namespace Core.Repository
{
    public class DocumentRepository : Repository<Document>
    {
        public Document GetByIdFull(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<Document>(id);
                NHibernateUtil.Initialize(result.DocumentType);
                NHibernateUtil.Initialize(result.Owner);
                NHibernateUtil.Initialize(result.StockUnitList);

                return result;
            }
        }

        public IList<Document> GetAllFull(bool initLists)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<Document>().List();
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.DocumentType);
                    NHibernateUtil.Initialize(item.Owner);
                    if (initLists)
                        NHibernateUtil.Initialize(item.StockUnitList);
                }

                return result;
            }
        }
    }
}
