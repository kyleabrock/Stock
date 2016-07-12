﻿using System.ComponentModel;

namespace Stock.Core
{
    public enum Genders
    {
        [Description("Не указано")]
        Unknown = 0,
        [Description("Мужской")]
        Male = 1,
        [Description("Женский")]
        Female = 2
    }

    public enum StatusTypes
    {
        [Description("На складе")]
        AtStock = 0,
        [Description("В работе")]
        InWork = 1,
        [Description("Ожидает")]
        Waiting = 2,
        [Description("Списано")]
        Discarded = 3
    }

    public enum LogMessageType
    {
        [Description("Аудит входа")]
        LoginAudit = 0,
        [Description("Аудит выхода")]
        LogoutAudit = 1,
        [Description("Изменение записи")]
        EntityChange = 2,
        [Description("Информация")]
        Information = 3
    }
}
