using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Stock.Core.Domain
{
    public class Card : EntityBase, ILoggedEntity
    {
        private string _cardNumber = "";
        public virtual string CardNumber
        {
            get { return _cardNumber; } 
            set { _cardNumber = value; }
        }

        private string _cardName = "";
        public virtual string CardName
        {
            get { return _cardName; } 
            set { _cardName = value; }
        }

        public virtual bool IsDefault { get; set; }

        private DateTime _creationDate = SqlDateTime.MinValue.Value;
        public virtual DateTime CreationDate
        {
            get { return _creationDate; } 
            set { _creationDate = value; }
        }

        public virtual Staff Staff { get; set; }
        public virtual IList<StockUnit> StockUnitList { get; set; }

        private string _comments = "";
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