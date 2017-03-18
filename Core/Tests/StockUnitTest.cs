using System;
using NUnit.Framework;
using Stock.Core.Filter;
using Stock.Core.Repository;

namespace Stock.Core.Tests
{
    [TestFixture]
    public class StockUnitTest
    {
        private StockUnitRepository _stockUnitRepository;

        [SetUp]
        public void SetUpVoid()
        {
            _stockUnitRepository = new StockUnitRepository();
        }

        [Test]
        public void FindTestFinderIsNull()
        {
            Assert.That(() => _stockUnitRepository.Find(null), Throws.TypeOf<NullReferenceException>());
        }
    }
}
