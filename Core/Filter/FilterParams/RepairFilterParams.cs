using System;
using System.Collections.Generic;
using Stock.Core.Domain;

namespace Stock.Core.Filter.FilterParams
{
    public class RepairFilterParams : IFilterParams
    {
        public RepairFilterParams()
        {
            ClearFilter();
        }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public IEnumerable<UserAcc> User { get; set; }
        
        public void ClearFilter()
        {
            StartDateTime = DateTime.Now.AddYears(-20);
            EndDateTime = DateTime.Now;
            User = new List<UserAcc>();
        }
    }
}
