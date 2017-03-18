using System;

namespace Stock.Core.Domain
{
    public class UnitType : EntityBase
    {
        private string _typeName = String.Empty;
        public virtual string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        public virtual int Priority { get; set; }

        private string _comments = String.Empty;

        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var other = obj as UnitType;
            if (other == null)
                return false;

            return (Id == other.Id)
                && (TypeName == other.TypeName)
                && (Priority == other.Priority)
                && (Comments == other.Comments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Priority.GetHashCode());
                hashCode = (hashCode * 397) ^ (TypeName != null ? TypeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Comments != null ? Comments.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(UnitType other)
        {
            return (Id == other.Id)
                && string.Equals(TypeName, other.TypeName) 
                && string.Equals(Priority, other.Priority) 
                && string.Equals(Comments, other.Comments);
        }
        
    }
}