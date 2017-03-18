using System;
using System.Collections.Generic;
using Stock.Core.Domain;

namespace Stock.Core.Filter.FilterParams
{
    public class CardFilterParams : IFilterParams
    {
        public CardFilterParams()
        {
            ClearFilter();
        }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<string> Department { get; set; }
        
        public void ClearFilter()
        {
            StartDateTime = DateTime.Now.AddYears(-20);
            EndDateTime = DateTime.Now;
            Staff = new List<Staff>();
            Department = new List<string>();
        }
    }
}
