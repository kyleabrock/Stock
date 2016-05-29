namespace Core.Domain
{
    public class EntityBase
    {
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual bool IsNew
        {
            get { return _id == PlaceholderId; }
        }

        private const int PlaceholderId = -1;
        private int _id = PlaceholderId;

        public override bool Equals(object obj)
        {
            var other = obj as EntityBase;
            if (other == null)
                return false;

            return _id == other.Id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        protected bool Equals(EntityBase other)
        {
            return _id == other._id;
        }
    }
}
