using System;

namespace Stock.Core.Domain
{
    public class StockUnitFile : EntityBase
    {
        public virtual StockUnit StockUnit { get; set; }

        private string _fileName = String.Empty;
        public virtual string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private string _description = String.Empty;
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _comments = String.Empty;
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
    }
}
