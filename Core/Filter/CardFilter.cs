using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class CardFilter : IFilterBase
    {
        public Staff Staff { get; set; }
        public string Department { get; set; }
        
        public void ClearFilter()
        {
            Staff = null;
            Department = null;
        }
    }
}
