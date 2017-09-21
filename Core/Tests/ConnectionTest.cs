using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Stock.Core.Domain;

namespace Stock.Core.Tests
{
    [TestFixture]
    public class ConnectionTest
    {
        private Configuration _configuration;
        private ISessionFactory _sessionFactory;

        /// <summary>
        /// Настройка подключения к базе данных
        /// </summary>
        [SetUp]
        public void Configure()
        {
            _configuration = new Configuration().DataBaseIntegration(db =>
            {
                var conn = new StringBuilder();
                conn.Append("Data Source=db.ktm.ossi.loc;");
                conn.Append("Initial Catalog=Stock;");
                conn.Append("User ID=developer;");
                conn.Append("Password=Pf,hfkj");

                db.Driver<SqlClientDriver>();
                db.ConnectionString = conn.ToString();
                db.Dialect<MsSql2008Dialect>();
            });
            
            _configuration.AddAssembly(typeof(EntityBase).Assembly);
            _sessionFactory = _configuration.BuildSessionFactory();
        }

        /// <summary>
        /// Генерирование новой базы данных
        /// </summary>
        [Test]
        public void Can_generate_schema()
        {
            new SchemaExport(_configuration).Execute(true, false, false);
        }

        /// <summary>
        /// Генерирование изменений в базу данных относительно существующей
        /// </summary>
        [Test]
        public void Can_update_schema()
        {
            new SchemaUpdate(_configuration).Execute(true, false);
        }
    }
}
