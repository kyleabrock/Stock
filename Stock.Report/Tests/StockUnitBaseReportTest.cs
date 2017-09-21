using NUnit.Framework;
using Stock.Core;
using Stock.Core.Repository;

namespace Stock.Report.Tests
{
    [TestFixture]
    class StockUnitBaseReportTest
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
        public void ExportBasePassport()
        {
            const string templatePath = "C:\\Work\\Паспорт.docx";
            const string outPath = "C:\\Work\\Результат.docx";

            var repository = new StockUnitRepository();
            var stockUnit = repository.GetById(662, true);

            var report = new StockUnitBaseReport();
            Assert.True(report.Export(stockUnit, templatePath, outPath));
        }
    }
}
