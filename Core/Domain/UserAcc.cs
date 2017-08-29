using System;

namespace Stock.Core.Domain
{
    public class UserAcc : EntityBase
    {
        public virtual Account Account { get; set; }

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

        private string _userImagePath = String.Empty;
        public virtual string UserImagePath
        {
            get { return _userImagePath; }
            set { _userImagePath = value; }
        }

        private string _comments = String.Empty;
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public static UserAcc Guest()
        {
            var result = new UserAcc
                {
                    Name = new Name {DisplayName = "Гость"}
                };

            return result;
        }
    }
}