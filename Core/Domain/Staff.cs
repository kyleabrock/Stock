namespace Core.Domain
{
    public class Staff : EntityBase
    {
        public virtual Name Name { get; set; }
        public virtual string Department { get; set; }
        public virtual string Comments { get; set; }
    }
}