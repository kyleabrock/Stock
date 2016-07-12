using System;
using NUnit.Framework;
using Stock.Core.Filter;
using Stock.Core.Finder;
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
        public void ComplexFilterTest()
        {
            var stockUnitRepository = new StockUnitRepository();
            var statusRepository = new StatusRepository();
            var ownerRepository = new OwnerRepository();
            var cardRepository = new CardRepository();

            var status = statusRepository.GetById(2);
            var owner = ownerRepository.GetById(1);
            var complexFilter = new StockUnitFilter {Status = status, Owner = owner};
            
            var stockUnitList = stockUnitRepository.GetAllByComplexFilter(complexFilter);
            Assert.Greater(stockUnitList.Count, 0);
        }

        [Test]
        public void FindTest()
        {
            var finder = new StockUnitFinder();
            finder.CreateCriteria("110");
            
            var list = _stockUnitRepository.Find(finder);

            Console.WriteLine("Count: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderCriteriaIsNull()
        {
            var finder = new StockUnitFinder();
            var list = _stockUnitRepository.Find(finder);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderSearchStringIsNull()
        {
            var finder = new StockUnitFinder();
            finder.CreateCriteria(null);
            var list = _stockUnitRepository.Find(finder);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderSearchStringIsEmpty()
        {
            var finder = new StockUnitFinder();
            finder.CreateCriteria("");
            var list = _stockUnitRepository.Find(finder);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderIsNull()
        {
            Assert.That(() => _stockUnitRepository.Find(null), Throws.TypeOf<NullReferenceException>());
        }
    }
}
