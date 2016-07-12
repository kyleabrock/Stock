namespace Stock.Core.Domain
{
    public class Status : EntityBase
    {
        public virtual StatusTypes StatusType { get; set; }

        private string _statusName = "";
        public virtual string StatusName
        {
            get { return _statusName; }
            set { _statusName = value; }
        }
        
        private string _comments = "";
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
    }
}