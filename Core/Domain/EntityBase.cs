using System;

namespace Stock.Core.Domain
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

        private Guid _shadowGuid = Guid.NewGuid();
        public virtual Guid ShadowGuid
        {
            get { return _shadowGuid; }
        }

        private const int PlaceholderId = -1;
        private int _id = PlaceholderId;

        public override bool Equals(object obj)
        {
            var other = obj as EntityBase;
            if (other == null)
                return false;

            if (other.IsNew)
                return ShadowGuid == other.ShadowGuid;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            if (IsNew)
                return ShadowGuid.GetHashCode();

            return Id.GetHashCode();
        }

        protected bool Equals(EntityBase other)
        {
            if (other.IsNew)
                return ShadowGuid == other.ShadowGuid;
            return Id == other.Id;
        }
    }
}
