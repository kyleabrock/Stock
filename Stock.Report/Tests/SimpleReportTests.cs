using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Stock.Core.Repository;

namespace Stock.Report.Tests
{
    [TestFixture]
    class SimpleReportTests
    {
        [Test]
        public void ReportExportTest()
        {
            var stockUnitRepository = new StockUnitRepository();
            var stockUnit = stockUnitRepository.GetById(1266, true);

            var templateFileName = "C:\\Work\\Паспорт.docx";
            var outFileName = "C:\\Work\\";

            var stockUnitReport = new SimpleReport();
            stockUnitReport.Export(stockUnit, templateFileName, outFileName);
        }
    }
}
