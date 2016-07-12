using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class UnitFilter : IFilterBase
    {
        public string Manufacture { get; set; }
        public string ModelName { get; set; }
        public UnitType UnitType { get; set; }
        public Owner Owner { get; set; }
        
        public void ClearFilter()
        {
            Manufacture = null;
            ModelName = null;
            UnitType = null;
            Owner = null;
        }
    }
}
