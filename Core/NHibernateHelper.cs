using System;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using Stock.Core.Domain;

namespace Stock.Core
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static bool Configure(string dbDataSource, string dbInitialCatalog, string dbUserId, string dbPassword, bool integratedSecurity)
        {
            try
            {
                var configuration = ReadConfigFromCacheFileOrBuildIt(dbDataSource, dbInitialCatalog, dbUserId, dbPassword, integratedSecurity);
                _sessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
            return true;
        }

        public static bool TestConnection()
        {
            return true;
            //var configuration = new Configuration();
            //try
            //{
            //    configuration.Configure();
            //}
            //catch (HibernateConfigException ex)
            //{
            //    LastError = ex.Message;
            //    return false;
            //}
            //try
            //{
            //    configuration.AddAssembly(typeof(Unit).Assembly);
            //}
            //catch (MappingException ex)
            //{
            //    LastError = ex.Message;
            //    return false;
            //}
            //try
            //{
            //    _sessionFactory = configuration.BuildSessionFactory();
            //}
            //catch (Exception ex)
            //{
            //    LastError = ex.Message;
            //    return false;
            //}
            //return true;
        }

        public static string LastError { get; set; }

        public static void ExportSchema()
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Unit).Assembly);

            new SchemaExport(configuration).Execute(false, true, false);
        }

        private static Configuration BuildConfiguration(string dbDataSource, string dbInitialCatalog, string dbUserId, string dbPassword, bool integratedSecurity)
        {
            var configuration = new Configuration().DataBaseIntegration(db =>
            {
                var conn = new StringBuilder();
                conn.Append("Data Source=");
                conn.Append(dbDataSource);
                conn.Append(";Initial Catalog=");
                conn.Append(dbInitialCatalog);
                if (integratedSecurity)
                    conn.Append(";Integrated Security=True");
                else
                {
                    conn.Append(";User ID=");
                    conn.Append(dbUserId);
                    conn.Append(";Password=");
                    conn.Append(dbPassword);
                }

                db.Driver<SqlClientDriver>();
                db.ConnectionString = conn.ToString();
                db.Dialect<MsSql2008Dialect>();
            });

            try
            {
                configuration.AddAssembly(typeof(EntityBase).Assembly);
                return configuration;
            }
            catch (MappingException ex)
            {
                LastError = ex.Message;
                return null;
            }
        }

        private static Configuration ReadConfigFromCacheFileOrBuildIt(string dbDataSource, string dbInitialCatalog, string dbUserId, string dbPassword, bool integratedSecurity)
        {
            Configuration nhConfigurationCache;
            var nhCfgCache = new ConfigurationFileCache(typeof(EntityBase).Assembly);
            var cachedCfg = nhCfgCache.LoadConfigurationFromFile();
            if (cachedCfg == null)
            {
                nhConfigurationCache = BuildConfiguration(dbDataSource, dbInitialCatalog, dbUserId, dbPassword, integratedSecurity);
                nhCfgCache.SaveConfigurationToFile(nhConfigurationCache);
            }
            else
            {
                nhConfigurationCache = cachedCfg;
            }

            return nhConfigurationCache;
        }
    }
}
