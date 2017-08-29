using System.Collections.Generic;
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
            SetValue("DbDataSource", Settings.Default.DbDataSource);
            SetValue("DbInitialCatalog", Settings.Default.DbInitialCatalog);
            SetValue("DbUserId", Settings.Default.DbUserID);
            SetValue("DbPassword", Settings.Default.DbPassword);
            SetValue("IntegratedSecurity", Settings.Default.IntegratedSecurity);

            SetValue("RememberCreditentials", Settings.Default.RememberCreditentials);
            SetValue("LdapAuth", Settings.Default.LdapAuth);
            SetValue("UserId", Settings.Default.UserId);
            SetValue("UserPassword", Settings.Default.UserPassword);
        }

        public static void LoadSettings()
        {
            SetValue("RefreshPeriod", Settings.Default.RefreshPeriod);
            SetValue("SettingsAppFolder", Settings.Default.SettingsAppFolder);
            SetValue("TemplatesFolderPath", Settings.Default.TemplatesFolderPath);
            SetValue("ExportFolderPath", Settings.Default.ExportFolderPath);
            SetValue("StockUnitFilesFolder", Settings.Default.StockUnitFilesFolder);
        }

        public static void SaveConnectionSettings()
        {
            Settings.Default.DbDataSource = GetValue<string>("DbDataSource");
            Settings.Default.DbInitialCatalog = GetValue<string>("DbInitialCatalog");
            Settings.Default.DbUserID = GetValue<string>("DbUserId");
            Settings.Default.DbPassword = GetValue<string>("DbPassword");
            Settings.Default.IntegratedSecurity = GetValue<bool>("IntegratedSecurity");

            Settings.Default.Save();
        }

        public static void SaveUserCreditentials()
        {
            Settings.Default.RememberCreditentials = GetValue<bool>("RememberCreditentials");
            Settings.Default.LdapAuth = GetValue<bool>("LdapAuth");
            Settings.Default.UserId = GetValue<string>("UserId");
            Settings.Default.UserPassword = GetValue<string>("UserPassword");

            Settings.Default.Save();
        }
    }
}
