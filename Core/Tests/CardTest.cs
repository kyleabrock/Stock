using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Stock.Core.Filter;
using Stock.Core.Repository;

namespace Stock.Core.Tests
{
    [TestFixture]
    public class CardTest
    {
        [Test]
        public void GetCardByStaffTest()
        {
            var cardRepository = new CardRepository();
            var staffRepository = new StaffRepository();
            var staff = staffRepository.GetById(1);

            var filter = new CardFilter {Staff = staff};
            var cardList = cardRepository.GetAllByComplexFilter(filter);

            foreach (var card in cardList)
                Console.WriteLine(card.CardNumber);

            Assert.Greater(cardList.Count, 0);
        }

        [Test]
        public void GetCardByDepartmentTest()
        {
            var cardRepository = new CardRepository();
            var staffRepository = new StaffRepository();
            var departments = staffRepository.GetDepartments();
            var department = departments.First();

            var filter = new CardFilter { Department = department };
            var cardList = cardRepository.GetAllByComplexFilter(filter);
            
            foreach (var card in cardList)
                Console.WriteLine(card.CardNumber);

            Assert.Greater(cardList.Count, 0);
        }

        [Test]
        public void GetCardByFullFilterTest()
        {
            var cardRepository = new CardRepository();
            var staffRepository = new StaffRepository();
            var staff = staffRepository.GetById(1);
            var department = "ОССИ";

            var filter = new CardFilter { Staff = staff, Department = department };
            var cardList = cardRepository.GetAllByComplexFilter(filter);

            foreach (var card in cardList)
                Console.WriteLine(card.CardNumber);

            Assert.Greater(cardList.Count, 0);
        }
    }
}
