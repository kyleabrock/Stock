using System;

namespace Core.Domain
{
    public class Unit : EntityBase
    {
        public virtual StockUnit StockUnit { get; set; }
        public virtual UnitType UnitType { get; set; }
        public virtual string Manufacture { get; set; }
        public virtual string ModelName { get; set; }
        public virtual string Serial { get; set; }
        public virtual string Comments { get; set; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var other = obj as Unit;
            if (other == null)
                return false;

            return (Manufacture == other.Manufacture) 
                && (ModelName == other.ModelName)
                && (Serial == other.Serial) 
                && (Comments == other.Comments);
        }

        protected bool Equals(Unit other)
        {
            return base.Equals(other)
                && string.Equals(Manufacture, other.Manufacture)
                && string.Equals(ModelName, other.ModelName) 
                && string.Equals(Serial, other.Serial) 
                && string.Equals(Comments, other.Comments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Manufacture != null ? Manufacture.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ModelName != null ? ModelName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Serial != null ? Serial.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Comments != null ? Comments.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}