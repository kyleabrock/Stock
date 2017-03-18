using System;

namespace Stock.Core.Domain
{
    public class Unit : EntityBase, ILoggedEntity
    {
        public virtual StockUnit StockUnit { get; set; }
        public virtual UnitType UnitType { get; set; }

        private string _manufacture = String.Empty;
        public virtual string Manufacture
        {
            get { return _manufacture; }
            set { _manufacture = value; }
        }

        private string _modelName = String.Empty;
        public virtual string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }

        private string _serial = String.Empty;
        public virtual string Serial
        {
            get { return _serial; }
            set { _serial = value; }
        }
        
        private string _comments = String.Empty;
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public virtual string FullModelName
        {
            get { return Manufacture + " " + ModelName; }
        }

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

        public virtual string LoggedMessage
        {
            get
            {
                string result = "Устройство. ";
                result += "ID: " + Id + "; ";
                result += "Тип: " + UnitType.TypeName + "; ";
                result += "Модель: " + FullModelName + "; ";
                result += "Сер. №: " + Serial + "; ";

                return result;
            }
        }
    }
}