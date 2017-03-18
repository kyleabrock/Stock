using System;
using System.Collections.Generic;

namespace Stock.Core.Domain
{
    public class Document : EntityBase, ILoggedEntity
    {
        public virtual DocumentType DocumentType { get; set; }

        private DocumentNumber _documentNumber = new DocumentNumber();
        public virtual DocumentNumber DocumentNumber
        {
            get { return _documentNumber; }
            set { _documentNumber = value; }
        }

        public virtual Owner Owner { get; set; }
        public virtual IList<StockUnit> StockUnitList { get; set; }

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
                string result = "Документ. ";
                result += "ID: " + Id + "; ";
                result += "Название: " + DocumentType.TypeName + "; ";
                result += "Номер: " + DocumentNumber.FullNumber + "; ";

                return result;
            }
        }
    }
}