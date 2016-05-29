using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Card : EntityBase
    {
        public virtual string CardNumber { get; set; }
        public virtual string CardName { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual IList<StockUnit> StockUnitList { get; set; }
        public virtual string Comments { get; set; }
    }
}