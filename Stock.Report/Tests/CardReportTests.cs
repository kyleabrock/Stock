using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Stock.Core.Repository;

namespace Stock.Report.Tests
{
    [TestFixture]
    public class CardReportTests
    {
        [Test]
        public void CardReportExportTest()
        {
            var cardRepository = new CardRepository();
            var card = cardRepository.GetById(23, true);

            var templateFileName = "C:\\Work\\Stock\\Stock.Report\\bin\\Debug\\Templates\\passportTemplate.docx";
            var outFileName = "C:\\Work\\Stock\\Stock.Report\\bin\\Debug\\Export\\passportTemplate.docx";
            
            var cardReport = new CardReport();
            cardReport.Export(templateFileName, outFileName, card, false);
        }
    }
}
