using System;
using NUnit.Framework;
using Stock.Core;
using Stock.Core.Domain;
using Stock.Core.Repository;

namespace Stock.UI.Tests
{
    [TestFixture]
    class AppSettingsTests
    {
        /// <summary>
        /// Настройка подключения к базе данных
        /// </summary>
        [SetUp]
        public void Configure()
        {
            NHibernateHelper.Configure("db.ktm.ossi.loc", "Stock", "developer", "Pf,hfkj", false);
        }

        [Test]
        public void AppSettingsGreaterZeroTest()
        {
            const int maltsevId = 1;
            var repository = new Repository<Account>();
            var account = repository.GetById(maltsevId);

            AppSettings.Account = account;
            AppSettings.Load();

            Assert.Greater(AppSettings.Count, 0);
        }

        [Test]
        public void GetSettingOfTypeDouble()
        {
            const int maltsevId = 1;
            var repository = new Repository<Account>();
            var account = repository.GetById(maltsevId);

            AppSettings.Account = account;
            AppSettings.Load();

            var value = Convert.ToDouble(AppSettings.GetValue<string>("CardTableDisplayNameWidth"));

            Assert.Greater(value, 0);
        }
    }
}
