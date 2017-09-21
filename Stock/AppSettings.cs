using System;
using System.Collections.Generic;
using Stock.Core.Domain;
using Stock.Core.Repository;

namespace Stock.UI
{
    public static class AppSettings
    {
        private static readonly Dictionary<string, object> UserSettings = 
            new Dictionary<string, object>();

        private static UserSettings _settings;

        public static Account Account { get; set; }
        public static UserAcc User { get; set; }

        public static void SetValue(string key, object value)
        {
            //if (UserSettings.ContainsKey(key))
            //    UserSettings.Remove(key);
            //UserSettings.Add(key, value);
            _settings[key] = value.ToString();
        }

        public static T GetValue<T>(string key)
        {
            if (UserSettings.ContainsKey(key))
                return (T) UserSettings[key];
            return default(T);
        }

        public static double GetAsDouble(string key)
        {
            if (_settings.ContainsKey(key))
                return double.Parse(_settings[key]);
            return default(double);
        }

        public static int GetAsInt(string key)
        {
            if (_settings.ContainsKey(key))
                return int.Parse(_settings[key]);
            return default(int);
        }

        public static int Count
        {
            get { return UserSettings.Count; }
        }

        public static void Load()
        {
            if (Account == null) throw new Exception("Account is null");
            
            var repository = new UserSettingRepository();
            _settings = repository.GetByAccount(Account);

            var defaultSettings = UserSettingRepository.Default(Account);
            if (_settings.Count == 0 || _settings.Count != defaultSettings.Count)
                _settings = defaultSettings;
        }

        public static void Save()
        {
            var repository = new UserSettingRepository();
            repository.Save(_settings);
        }
    }
}
