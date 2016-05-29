namespace Core.Domain
{
    public class DocumentType : EntityBase
    {
        public virtual string TypeName { get; set; }        
        public virtual string Comments { get; set; }
    }
}