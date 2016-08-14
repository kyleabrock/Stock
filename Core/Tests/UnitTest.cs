using System;
using NUnit.Framework;
using Stock.Core.Finder;
using Stock.Core.Repository;

namespace Stock.Core.Tests
{
    [TestFixture]
    public class UnitTest
    {
        private UnitRepository _unitRepository;

        [SetUp]
        public void Init()
        {
            _unitRepository = new UnitRepository();
        }

        [TearDown]
        public void Dispose()
        {
            _unitRepository = null;
        }

        [Test]
        public void GetManufactureTest()
        {
            var manufactureList = _unitRepository.GetManufactureList();
            Console.WriteLine("Total items: " + manufactureList.Count);
            foreach (var item in manufactureList)
                Console.WriteLine(item);
            Assert.Greater(manufactureList.Count, 0);
        }

        [Test]
        public void GetModelTest()
        {
            var modelList = _unitRepository.GetModelList();
            Console.WriteLine("Total items: " + modelList.Count);
            foreach (var item in modelList)
                Console.WriteLine(item);
            Assert.Greater(modelList.Count, 0);
        }

        [Test]
        public void GetAllFullTest()
        {
            var list = _unitRepository.GetAll(x=> x.StockUnit, true, false);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTest()
        {
            var finder = new UnitFinder();
            finder.CreateCriteria("Мед");
            
            var list = _unitRepository.Find(finder);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderCriteriaIsNull()
        {
            var finder = new UnitFinder();
            var list = _unitRepository.Find(finder);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderSearchStringIsNull()
        {
            var finder = new UnitFinder();
            finder.CreateCriteria(null);
            var list = _unitRepository.Find(finder);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderSearchStringIsEmpty()
        {
            var finder = new UnitFinder();
            finder.CreateCriteria("");
            var list = _unitRepository.Find(finder);

            Console.WriteLine("Returned objects: " + list.Count);
            Assert.Greater(list.Count, 0);
        }

        [Test]
        public void FindTestFinderIsNull()
        {
            Assert.That(() => _unitRepository.Find(null), Throws.TypeOf<NullReferenceException>());
        }
    }
}
