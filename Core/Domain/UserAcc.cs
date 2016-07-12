namespace Stock.Core.Domain
{
    public class UserAcc : EntityBase
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

        private string _userImagePath = "";
        public virtual string UserImagePath
        {
            get { return _userImagePath; }
            set { _userImagePath = value; }
        }

        private string _comments = "";
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public virtual Account Account { get; set; }
    }
}