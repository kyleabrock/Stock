using System.Collections.Generic;
using Stock.Core.Domain;

namespace Stock.Core.Filter.FilterParams
{
    public class UnitFilterParams : IFilterParams
    {
        public UnitFilterParams()
        {
            ClearFilter();
        }

        public IEnumerable<Status> Status { get; set; }
        public IEnumerable<UnitType> UnitType { get; set; }
        public IEnumerable<string> Manufacture { get; set; }
        public IEnumerable<string> ModelName { get; set; }
        public IEnumerable<Owner> Owner { get; set; }
        
        public void ClearFilter()
        {
            Status = new List<Status>();
            Manufacture = new List<string>();
            ModelName = new List<string>();
            UnitType = new List<UnitType>();
            Owner = new List<Owner>();
        }
    }
}
