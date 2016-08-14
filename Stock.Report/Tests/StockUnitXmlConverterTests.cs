using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Stock.Core.Repository;

namespace Stock.Report.Tests
{
    [TestFixture]
    public class StockUnitXmlConverterTests
    {
        [Test]
        public void ConvertTest()
        {
            var stockUnitRepository = new StockUnitRepository();
            var stockUnit = stockUnitRepository.GetById(1266, true);

            var converter = new StockUnitXmlConverter();
            var result = converter.Convert(stockUnit);
            Console.WriteLine(result.OuterXml);
        }
    }
}
