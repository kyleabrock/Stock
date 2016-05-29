namespace Core.Domain
{
    public class Name
    {
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Patronymic { get; set; }
        public virtual string DisplayName { get; set; }
    }
}
