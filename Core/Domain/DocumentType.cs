namespace Stock.Core.Domain
{
    public class DocumentType : EntityBase
    {
        private string _typeName = "";
        public virtual string TypeName
        {
            get { return _typeName; } 
            set { _typeName = value; }
        }
        
        private string _comments = "";
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
    }
}