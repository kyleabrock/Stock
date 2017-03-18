using System;
using Stock.Core.Domain;

namespace Stock.Core.Factory
{
    public class LogFactory : ILogFactory
    {
        public Log CreateUserLoginMessage(UserAcc user)
        {
            var logEntity = new Log
            {
                MessageType = LogMessageType.LoginAudit,
                Date = DateTime.Now,
                UserId = user.Id,
                UserName = user.Name.DisplayName,
                Message = "Выполнен вход в систему"
            };

            return logEntity;
        }

        public Log CreateUserLogoutMessage(UserAcc user)
        {
            var logEntity = new Log
            {
                MessageType = LogMessageType.LogoutAudit,
                Date = DateTime.Now,
                UserId = user.Id,
                UserName = user.Name.DisplayName,
                Message = "Выполнен выход из системы"
            };

            return logEntity;
        }

        public Log CreateMessage(UserAcc user, ILoggedEntity entity)
        {
            var logEntity = new Log
            {
                MessageType = LogMessageType.EntityChange,
                Date = DateTime.Now,
                UserId = user.Id,
                UserName = user.Name.DisplayName,
                Message = entity.LoggedMessage 
            };

            return logEntity;
        }
    }
}
