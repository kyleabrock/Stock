using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class RepairFilter : IFilterBase
    {
        public UserAcc User { get; set; }
        
        public void ClearFilter()
        {
            User = null;
        }
    }
}
