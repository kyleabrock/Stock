namespace Core.Domain
{
    public class Status : EntityBase
    {
        public virtual StatusTypes StatusType { get; set; }
        public virtual string StatusName { get; set; }        
        public virtual string Comments { get; set; }
    }
}