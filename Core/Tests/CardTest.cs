using System;
using System.Linq;
using NUnit.Framework;
using Stock.Core.Filter;
using Stock.Core.Filter.FilterParams;
using Stock.Core.Repository;

namespace Stock.Core.Tests
{
    [TestFixture]
    public class CardTest
    {
        [Test]
        public void GetCardByStaffTest()
        {
            //var cardRepository = new CardRepository();
            //var staffRepository = new StaffRepository();
            //var staff = staffRepository.GetById(1);

            //var filter = new CardFilterParams {Staff = staff};
            //var cardList = cardRepository.GetAllByComplexFilter(filter);

            //foreach (var card in cardList)
            //    Console.WriteLine(card.CardNumber);

            //Assert.Greater(cardList.Count, 0);
        }

        [Test]
        public void GetCardByDepartmentTest()
        {
            //var cardRepository = new CardRepository();
            //var staffRepository = new StaffRepository();
            //var departments = staffRepository.GetDepartments();
            //var department = departments.First();

            //var filter = new CardFilterParams { Department = department };
            //var cardList = cardRepository.GetAllByComplexFilter(filter);
            
            //foreach (var card in cardList)
            //    Console.WriteLine(card.CardNumber);

            //Assert.Greater(cardList.Count, 0);
        }

        [Test]
        public void GetCardByFullFilterTest()
        {
            //var cardRepository = new CardRepository();
            //var staffRepository = new StaffRepository();
            //var staff = staffRepository.GetById(1);
            //var department = "ОССИ";

            //var filter = new CardFilterParams { Staff = staff, Department = department };
            //var cardList = cardRepository.GetAllByComplexFilter(filter);

            //foreach (var card in cardList)
            //    Console.WriteLine(card.CardNumber);

            //Assert.Greater(cardList.Count, 0);
        }
    }
}
