using System.Collections.Generic;
using Core.Domain;
using NHibernate;

namespace Core.Repository
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

        public T GetById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.Get<T>(id);
            }
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
