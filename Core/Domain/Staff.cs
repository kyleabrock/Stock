using System;

namespace Stock.Core.Domain
{
    public class Staff : EntityBase, ILoggedEntity
    {
        private Name _name = new Name();
        public virtual Name Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _department = String.Empty;
        public virtual string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        private string _comments = String.Empty;
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public virtual string LoggedMessage
        {
            get
            {
                string result = "Пользователь. ";
                result += "ID: " + Id + "; ";
                result += "Ф.И.О.: " + Name.DisplayName + "; ";
                result += "Отдел: " + Department + "; ";

                return result;
            }
        }
    }
}