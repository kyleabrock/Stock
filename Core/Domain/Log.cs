using System;

namespace Stock.Core.Domain
{
    public class Log : EntityBase
    {
        public virtual LogMessageType MessageType { get; set; }

        private DateTime _date = DateTime.Now;
        public virtual DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual int UserId { get; set; }

        private string _userName = "";
        public virtual string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _message = "";
        public virtual string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public virtual string MessageTypeAsString
        {
            get
            {
                switch (MessageType)
                {
                    case LogMessageType.LoginAudit:
                        return "Аудит входа";
                    case LogMessageType.LogoutAudit:
                        return "Аудит выхода";
                    case LogMessageType.EntityChange:
                        return "Изменение записи";
                    case LogMessageType.Information:
                        return "Информация";
                    default:
                        return "Запись";
                }
            }
        }
    }
}
