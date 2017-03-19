using System;
using System.Collections.Generic;
using System.IO;
using Stock.Core;
using Stock.UI.Properties;

namespace Stock.UI
{
    public static class ApplicationState
    {
        private static readonly Dictionary<string, object> Values = 
            new Dictionary<string, object>();

        public static void SetValue(string key, object value)
        {
            if (Values.ContainsKey(key))
                Values.Remove(key);
            Values.Add(key, value);
        }

        public static T GetValue<T>(string key)
        {
            if (Values.ContainsKey(key))
                return (T) Values[key];
            return default(T);
        }

        public static void LoadConnectionSettings()
        {
            var dbDataSource = Settings.Default["DbDataSource"].ToString();
            var dbInitialCatalog = Settings.Default["DbInitialCatalog"].ToString();
            var dbUserId = Settings.Default["DbUserId"].ToString();
            var dbPassword = Settings.Default["DbPassword"].ToString();
            var integratedSecurity = (bool) Settings.Default["IntegratedSecurity"];

            SetValue("DbDataSource", dbDataSource);
            SetValue("DbInitialCatalog", dbInitialCatalog);
            SetValue("DbUserId", dbUserId);
            SetValue("DbPassword", dbPassword);
            SetValue("IntegratedSecurity", integratedSecurity);

            var rememberCreditentials = (bool) Settings.Default["RememberCreditentials"];
            var ldapAuth = (bool) Settings.Default["LdapAuth"];
            var userId = Settings.Default["UserId"];
            var userPassword = Settings.Default["UserPassword"];

            SetValue("RememberCreditentials", rememberCreditentials);
            SetValue("LdapAuth", ldapAuth);
            SetValue("UserId", userId);
            SetValue("UserPassword", userPassword);
        }

        public static void LoadSettings()
        {
            string settingAppDirectory = Settings.Default["SettingsAppFolder"].ToString();
            string templatesFolderPath = Settings.Default["TemplatesFolderPath"].ToString();
            string exportFolderPath = Settings.Default["ExportFolderPath"].ToString();
            string stockUnitFilesFolder = Settings.Default["StockUnitFilesFolder"].ToString();
            
            SetValue("RefreshPeriod", (int)Settings.Default["RefreshPeriod"]);
            SetValue("SettingsAppFolder", settingAppDirectory);
            SetValue("TemplatesFolderPath", templatesFolderPath);
            SetValue("ExportFolderPath", exportFolderPath);
            SetValue("StockUnitFilesFolder", stockUnitFilesFolder);
        }

        public static void SaveConnectionSettings()
        {
            Settings.Default["DbDataSource"] = GetValue<string>("DbDataSource");
            Settings.Default["DbInitialCatalog"] = GetValue<string>("DbInitialCatalog");
            Settings.Default["DbUserId"] = GetValue<string>("DbUserId");
            Settings.Default["DbPassword"] = GetValue<string>("DbPassword");
            Settings.Default["IntegratedSecurity"] = GetValue<bool>("IntegratedSecurity");

            Settings.Default.Save();
        }

        public static void SaveUserCreditentials()
        {
            Settings.Default["RememberCreditentials"] = GetValue<bool>("RememberCreditentials");
            Settings.Default["LdapAuth"] = GetValue<bool>("LdapAuth");
            Settings.Default["UserId"] = GetValue<string>("UserId");
            Settings.Default["UserPassword"] = GetValue<string>("UserPassword");

            Settings.Default.Save();
        }
    }
}
