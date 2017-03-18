using System;
using System.Collections.Generic;

namespace Stock.Core.Domain
{
    public class Card : EntityBase, ILoggedEntity
    {
        private string _cardNumber = String.Empty;
        public virtual string CardNumber
        {
            get { return _cardNumber; } 
            set { _cardNumber = value; }
        }

        private string _cardName = String.Empty;
        public virtual string CardName
        {
            get { return _cardName; } 
            set { _cardName = value; }
        }

        public virtual bool IsDefault { get; set; }

        private DateTime _creationDate = DateTime.Now;
        public virtual DateTime CreationDate
        {
            get { return _creationDate; } 
            set { _creationDate = value; }
        }

        public virtual Staff Staff { get; set; }

        private IList<StockUnit> _stockUnitList = new List<StockUnit>();
        public virtual IList<StockUnit> StockUnitList
        {
            get { return _stockUnitList; }
            set { _stockUnitList = value; }
        }

        private string _comments = String.Empty;
        public virtual string Comments
        {
            get { return _comments; } 
            set { _comments = value; }
        }

        public virtual string LoggedMessage
        {
            get
            {
                string result = "Карточка. ";
                result += "ID: " + Id + "; ";
                result += "Номер: " + CardNumber + "; ";
                result += "Название: " + CardName + "; ";
                
                return result;
            }
        }
    }
}