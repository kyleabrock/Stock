using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class StockUnitFilter : IFilterBase
    {
        public Owner Owner { get; set; }
        public Card Card { get; set; }
        public Status Status { get; set; }
        
        public void ClearFilter()
        {
            Owner = null;
            Card = null;
            Status = null;
        }
    }
}
