using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class StockUnit : EntityBase
    {
        public virtual string StockNumber { get; set; }
        public virtual string StockName { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual Card Card { get; set; }
        public virtual Status Status { get; set; }
        public virtual IList<Document> DocumentList { get; set; }
        public virtual IList<Unit> UnitList { get; set; }
        public virtual string Comments { get; set; }

        public virtual void AddUnit(Unit item)
        {
            item.StockUnit = this;
            if (UnitList == null) 
                UnitList = new List<Unit>();
            UnitList.Add(item);
        }

        public virtual void AddUnit(IList<Unit> items)
        {
            foreach (var item in items)
                AddUnit(item);
        }

        public virtual void RemoveUnit(Unit item)
        {
            UnitList.Remove(item);
        }

        public virtual void RemoveUnit(IList<Unit> items)
        {
            foreach (var item in items)
                RemoveUnit(item);
        }
    }
}