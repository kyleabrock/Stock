using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using Stock.Core.Domain;
using Stock.Core.Filter;
using IFilter = Stock.Core.Filter.IFilter;

namespace Stock.Core.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        public IList<T> GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<T>().List();
            }
        }

        public IList<T> GetAll(Expression<Func<T, object>> orderByPath, bool asc = true)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var query = session.QueryOver<T>();
                var result = asc ? query.OrderBy(orderByPath).Asc.List() 
                    : query.OrderBy(orderByPath).Desc.List();
                
                return result;
            }
        }

        public T GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public virtual IList<T> GetAllAsTableView()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<T>().List();
                InitializeTableValues(result);
                return result;
            }
        }

        public virtual IList<T> Find(IFilter filter)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var criteria = filter.Criteria;
                if (criteria != null)
                {
                    var result = criteria.GetExecutableCriteria(session).List<T>();
                    InitializeTableValues(result);
                    return result;
                }

                return GetAllAsTableView();
            }
        }

        protected virtual void InitializeTableValues(IEnumerable<T> items)
        {
            
        }

        public void Save(T arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (arg.IsNew)
                        session.Save(arg);
                    else
                    {
                        session.Update(arg);
                    }
                    transaction.Commit();
                }
            }
        }

        public void Save(IEnumerable<T> arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (var item in arg)
                    {
                        if (item.IsNew)
                            session.Save(item);
                        else
                        {
                            session.Update(item);
                        }
                    }
                    
                    transaction.Commit();
                }
            }
        }

        public void Add(T arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(arg);
                    transaction.Commit();
                }
            }
        }

        public void Add(IEnumerable<T> arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (var item in arg)
                        session.Save(item);
                    transaction.Commit();
                }
            }
        }

        public void Update(T arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(arg);
                    transaction.Commit();
                }
            }
        }

        public void Update(IEnumerable<T> arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (var item in arg)
                        session.Update(item);
                    transaction.Commit();
                }
            }
        }

        public void Delete(T arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(arg);
                    transaction.Commit();
                }
            }
        }

        public void Delete(IEnumerable<T> arg)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (var item in arg)
                        session.Delete(item);
                    transaction.Commit();
                }
            }
        }
    }
}
