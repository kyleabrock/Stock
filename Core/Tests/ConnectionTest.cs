using System;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;
using Stock.Core.Domain;

namespace Stock.Core.Tests
{
    [TestFixture]
    public class ConnectionTest
    {
        [Test]
        public void BuildConfiguration()
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Unit).Assembly);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();

            Assert.IsNotNull(sessionFactory);
        }

        [Test]
        public void BuildConfigurationCfgFileNotExists()
        {
            var configuration = new Configuration();
            Assert.That(() => configuration.Configure("nullfile"), Throws.TypeOf<HibernateConfigException>());
        }

        [Test]
        public void BuildConfigurationNotConfiguredNotMapped()
        {
            var configuration = new Configuration();
            Assert.That(() => configuration.BuildSessionFactory(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void BuildConfigurationNotConfigured()
        {
            var configuration = new Configuration();
            Assert.That(() => configuration.AddAssembly(typeof(Unit).Assembly), Throws.TypeOf<MappingException>());
        }
    }
}
