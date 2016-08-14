using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Stock.Core.Domain;

namespace Stock.Core
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddAssembly(typeof(Unit).Assembly);
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static bool TestConnection()
        {
            var configuration = new Configuration();
            try
            {
                configuration.Configure();
            }
            catch (HibernateConfigException ex)
            {
                LastError = ex.Message;
                return false;
            }
            try
            {
                configuration.AddAssembly(typeof(Unit).Assembly);
            }
            catch (MappingException ex)
            {
                LastError = ex.Message;
                return false;
            }
            try
            {
                _sessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
            return true;
        }

        public static string LastError { get; set; }

        public static void ExportSchema()
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Unit).Assembly);

            new SchemaExport(configuration).Execute(false, true, false);
        }
    }
}
