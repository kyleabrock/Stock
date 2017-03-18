using System;

namespace Stock.Core.Domain
{
    public class Status : EntityBase
    {
        public virtual StatusTypes StatusType { get; set; }

        private string _statusName = String.Empty;
        public virtual string StatusName
        {
            get { return _statusName; }
            set { _statusName = value; }
        }
        
        private string _comments = String.Empty;
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
    }
}