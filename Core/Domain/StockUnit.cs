using System;
using System.Collections.Generic;

namespace Stock.Core.Domain
{
    public class StockUnit : EntityBase, ILoggedEntity
    {
        private string _stockNumber = String.Empty;
        public virtual string StockNumber
        {
            get { return _stockNumber; }
            set { _stockNumber = value; }
        }

        private string _stockName = String.Empty;
        public virtual string StockName
        {
            get { return _stockName; }
            set { _stockName = value; }
        }

        private DateTime _creationDate = DateTime.Now;
        public virtual DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        public virtual Owner Owner { get; set; }
        public virtual Card Card { get; set; }
        public virtual Status Status { get; set; }
        public virtual IList<Document> DocumentList { get; set; }
        public virtual IList<Unit> UnitList { get; set; }
        
        private string _comments = String.Empty;
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public virtual void AddUnit(Unit item)
        {
            item.StockUnit = this;
            if (UnitList == null) 
                UnitList = new List<Unit>();
            UnitList.Add(item);
        }

        public virtual void AddUnit(IEnumerable<Unit> items)
        {
            foreach (var item in items)
                AddUnit(item);
        }

        public virtual void RemoveUnit(Unit item)
        {
            UnitList.Remove(item);
        }

        public virtual void RemoveUnit(IEnumerable<Unit> items)
        {
            foreach (var item in items)
                RemoveUnit(item);
        }

        public virtual string LoggedMessage
        {
            get
            {
                string result = "Основное средство. ";
                result += "ID: " + Id + "; ";
                result += "Инв. №: " + StockNumber + "; ";
                result += "Название: " + StockName + "; ";

                return result;
            }
        }
    }
}