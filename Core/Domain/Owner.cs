namespace Stock.Core.Domain
{
    public class Owner : EntityBase, ILoggedEntity
    {
        private Name _name = new Name();
        public virtual Name Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _department = "";
        public virtual string Department
        {
            get { return _department; }
            set { _department = value; }
        }
        
        private string _comments = "";
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public virtual string LoggedMessage
        {
            get
            {
                string result = "ÌÎË. ";
                result += "ID: " + Id + "; ";
                result += "Ô.È.Î.: " + Name.DisplayName + "; ";
                result += "Îòäåë: " + Department + "; ";

                return result;
            }
        }
    }
}