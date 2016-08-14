using System;
using System.Data.SqlTypes;

namespace Stock.Core.Domain
{
    public class Repair : EntityBase, ILoggedEntity
    {
        public virtual Unit Unit { get; set; }

        private DateTime _startedDate = DateTime.Now;
        public virtual DateTime StartedDate
        {
            get { return _startedDate; }
            set { _startedDate = value; }
        }

        private DateTime _completedDate = SqlDateTime.MinValue.Value;
        public virtual DateTime CompletedDate
        {
            get { return _completedDate; }
            set { _completedDate = value; }
        }

        private string _defect = "";
        public virtual string Defect
        {
            get { return _defect; }
            set { _defect = value; }
        }

        private string _result = "";
        public virtual string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public virtual UserAcc User { get; set; }

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
                string result = "Ремонт. ";
                result += "ID: " + Id + "; ";
                result += "Устройство: " + Unit.FullModelName + "; ";
                result += "Неисправность: " + Defect + "; ";
                result += "Результат: " + Result + "; ";

                return result;
            }
        }
    }
}
