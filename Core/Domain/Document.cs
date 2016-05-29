using System.Collections.Generic;

namespace Core.Domain
{
    public class Document : EntityBase
    {
        public virtual DocumentType DocumentType { get; set; }
        public virtual DocumentNumber DocumentNumber { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual IList<StockUnit> StockUnitList { get; set; }        
        public virtual string Comments { get; set; }
    }
}