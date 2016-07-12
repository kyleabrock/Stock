using System;
using NUnit.Framework;
using Stock.Core.Repository;

namespace Stock.Report.Tests
{
    [TestFixture]
    class StockUnitReportTests
    {
        [Test]
        public void ReportExportTest()
        {
            var stockUnitRepository = new StockUnitRepository();
            var stockUnit = stockUnitRepository.GetById(1266, true);

            var templateFileName = "C:\\Work\\Stock\\Stock.Report\\bin\\Debug\\Templates\\passport.docx";
            var outFileName = "C:\\Work\\Stock\\Stock.Report\\bin\\Debug\\Export\\stockUnitTemplate.docx";

            var stockUnitReport = new StockUnitReport();
            stockUnitReport.Export(stockUnit, templateFileName, outFileName);
        }

        [Test]
        public void ReportExportTemplateNotExistsTest()
        {
            var stockUnitRepository = new StockUnitRepository();
            var stockUnit = stockUnitRepository.GetById(1266, true);

            var templateFileName = "C:\\template.docx";
            var outFileName = "C:\\Work\\Stock\\Stock.Report\\bin\\Debug\\Export\\stockUnitTemplate.docx";

            var stockUnitReport = new StockUnitReport();
            var result = stockUnitReport.Export(stockUnit, templateFileName, outFileName);
            
            Console.WriteLine(stockUnitReport.LastError);
            Assert.IsTrue(!result);
        }

        [Test]
        public void ReportExportDirectoryNotExistTest()
        {
            var stockUnitRepository = new StockUnitRepository();
            var stockUnit = stockUnitRepository.GetById(1266, true);

            var templateFileName = "C:\\Work\\Stock\\Stock.Report\\bin\\Debug\\Templates\\passport.docx";
            var outFileName = "C:\\Work\\Stock\\Stock.Report\\bin\\Debug\\ExportTest\\test.docx";

            var stockUnitReport = new StockUnitReport();
            var result = stockUnitReport.Export(stockUnit, templateFileName, outFileName);
            
            Console.WriteLine(stockUnitReport.LastError);
            Assert.IsTrue(!result);
        }
    }
}