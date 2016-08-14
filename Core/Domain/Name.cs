﻿namespace Stock.Core.Domain
{
    public class Name
    {
        private string _lastName = "";
        public virtual string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private string _firstName = "";
        public virtual string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _patronymic = "";
        public virtual string Patronymic
        {
            get { return _patronymic; }
            set { _patronymic = value; }
        }

        private string _displayName = "";
        public virtual string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
