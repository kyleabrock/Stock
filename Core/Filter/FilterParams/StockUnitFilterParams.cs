using System;
using System.Collections.Generic;
using Stock.Core.Domain;

namespace Stock.Core.Filter.FilterParams
{
    public class StockUnitFilterParams : IFilterParams
    {
        public StockUnitFilterParams()
        {
            ClearFilter();
        }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public IEnumerable<Owner> Owner { get; set; }
        public IEnumerable<Card> Card { get; set; }
        public IEnumerable<Status> Status { get; set; }
        
        public void ClearFilter()
        {
            StartDateTime = DateTime.Now.AddYears(-20);
            EndDateTime = DateTime.Now;
            Owner = new List<Owner>();
            Card = new List<Card>();
            Status = new List<Status>();
        }
    }
}
