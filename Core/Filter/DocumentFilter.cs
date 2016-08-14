using Stock.Core.Domain;

namespace Stock.Core.Filter
{
    public class DocumentFilter : IFilterBase
    {
        public DocumentType DocumentType { get; set; }
        public Owner Owner { get; set; }
        
        public void ClearFilter()
        {
            DocumentType = null;
            Owner = null;
        }
    }
}
