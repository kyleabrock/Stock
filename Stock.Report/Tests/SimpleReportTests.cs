using System;
using System.IO;
using NUnit.Framework;

namespace Stock.Report.Tests
{
    [TestFixture]
    class SimpleReportTests
    {
        [Test]
        public void ReportExportTest()
        {
            //const string templateFileName = "C:\\Work\\Паспорт.docx";
            //const string outFileName = "C:\\Work\\";

            //var stockUnitRepository = new StockUnitRepository();
            //var stockUnit = stockUnitRepository.GetById(1266, true);

            //var simpleReport = new SimpleReport();
            //simpleReport.Export(stockUnit, templateFileName, outFileName);

            Environment.CurrentDirectory = @"D:\Документы";
            Console.WriteLine(Path.GetFullPath(@".\812.doc"));
        }
    }
}
