using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Stock.Core.Domain;
using Stock.Core.Filter;

namespace Stock.Core.Repository
{
    public class DocumentRepository : Repository<Document>, IComplexFilterRepository<Document>
    {
        public Document GetById(int id, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.Get<Document>(id);
                if (!eagerLoading)
                    return result;

                NHibernateUtil.Initialize(result.DocumentType);
                NHibernateUtil.Initialize(result.Owner);
                NHibernateUtil.Initialize(result.StockUnitList);

                return result;
            }
        }

        public IList<Document> GetAll(Expression<Func<Document, object>> orderByPath, bool asc, bool eagerLoading)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.QueryOver<Document>().OrderBy(orderByPath);
                var result = asc ? query.Asc.List() : query.Desc.List();

                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.DocumentType);
                    NHibernateUtil.Initialize(item.Owner);
                    if (eagerLoading)
                        NHibernateUtil.Initialize(item.StockUnitList);
                }

                return result;
            }
        }

        public IList<Document> GetByStockUnit(StockUnit stockUnit)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                if (!stockUnit.IsNew)
                {
                    var result = session.CreateCriteria<Document>()
                        .CreateCriteria("StockUnitList", "s")
                        .Add(Restrictions.Eq("s.Id", stockUnit.Id))
                        .List<Document>();

                    foreach (var item in result)
                    {
                        NHibernateUtil.Initialize(item.DocumentType);
                        NHibernateUtil.Initialize(item.Owner);
                    }
                    
                    return result;
                }

                return new List<Document>();
            }
        }

        public IList<Document> GetAllByComplexFilter(IFilterBase filter)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var mainCriteria = session.CreateCriteria<Document>();
                var documentFilter = filter as DocumentFilter;
                if (documentFilter != null)
                {
                    if (documentFilter.DocumentType != null)
                        mainCriteria.CreateCriteria("DocumentType").Add(Restrictions.Eq("Id", documentFilter.DocumentType.Id));
                    if (documentFilter.Owner != null)
                        mainCriteria.CreateCriteria("Owner").Add(Restrictions.Eq("Id", documentFilter.Owner.Id));
                }

                var result = mainCriteria.List<Document>();
                foreach (var item in result)
                {
                    NHibernateUtil.Initialize(item.DocumentType);
                    NHibernateUtil.Initialize(item.Owner);
                }

                return result;
            }
        }
    }
}
